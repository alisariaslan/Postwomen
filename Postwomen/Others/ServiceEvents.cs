using Microsoft.VisualBasic;
using Plugin.LocalNotification;
using Postwomen.Enums;
using Postwomen.Models;
using System.Net.NetworkInformation;

namespace Postwomen.Others;

public class ServiceEvents
{
	private PostwomenDatabase PostwomenDatabase { get; set; }
	private Ping Ping { get; set; }
	private int BadgeNumber { get; set; }
	private List<int> BadServers { get; set; }
	public ServiceEvents()
	{
		this.PostwomenDatabase = new PostwomenDatabase();
		this.Ping = new Ping();
		this.BadServers = new List<int>();
	}

	public async void Checkup()
	{
		BadServers.Clear();
		Console.WriteLine("(ServiceEvents:CF), Starting to check..");
		var dbItems = await PostwomenDatabase.GetItemsAsync();
		foreach (var item in dbItems)
		{
			if (item.IsAutoCheckEnabled is false)
				continue;
			switch (item.TypeOfCall)
			{
				case 1: await CheckPing(item); break;
				case 2: await CheckGET(item); break;
				case 3: await CheckPOST(item); break;
			}
		}
		if (BadServers.Count > 0)
		{
			string desc = "";
			foreach (var badswId in BadServers)
			{
				var sw = dbItems.Find(model => badswId == model.Id);
				desc += $"{sw.Name} - {sw.Url}:{sw.Port} - {GetCallType.Get(sw.TypeOfCall)},\n";
			}
			var request = new NotificationRequest
			{
				NotificationId = 10101010,
				Title = "Some servers has issues!",
				Subtitle = "Bad servers",
				Description = desc,
				BadgeNumber = this.BadgeNumber++,
				Group = "General",
				CategoryType = NotificationCategoryType.None
			};
			await LocalNotificationCenter.Current.Show(request);
		}
	}

	private async Task CheckPing(ServerModel model)
	{
		var success = false;
		var ping_string = "ping -c 1 " + model.Url;
		try
		{
#if ANDROID
        Java.Lang.Process p1 = Java.Lang.Runtime.GetRuntime().Exec(ping_string);
        int returnVal = p1.WaitFor();
		if(returnVal == 1)
			success = true;
#endif
			if (success is false)
			{
				PingReply reply = await Ping.SendPingAsync(model.Url);
				Console.WriteLine("(ServiceEvents:CP), Ping result: " + reply.Status + " - " + model.Name);
				if (reply.Status == IPStatus.Success)
					success = true;
			}
		}
		catch (Exception e)
		{
			Console.WriteLine("(ServiceEvents:CP)! Exception occured: " + e.Message);
		}
		if (success is false && model.IsSendNotificationsOnChangesEnabled)
		{
			model.CurrentState = (int)CheckStates.UNREACHABLE;
			await PostwomenDatabase.SaveItemAsync(model);
			if (model.IsSeperatedFromGeneralNotifications is false)
				BadServers.Add(model.Id);
			else
			{
				var request = new NotificationRequest
				{
					NotificationId = model.Id,
					Title = $"{model.Name} has issues!",
					Subtitle = "Bad servers",
					Description = $"{model.Url}:{model.Port} - {GetCallType.Get(model.TypeOfCall)}",
					BadgeNumber = this.BadgeNumber++,
					Group = model.Name,
					CategoryType = NotificationCategoryType.None
				};
				await LocalNotificationCenter.Current.Show(request);
			}
		} else if(success)
		{
			model.CurrentState = (int)CheckStates.OK;
			await PostwomenDatabase.SaveItemAsync(model);
		}
		
	}

	private async Task CheckGET(ServerModel model)
	{

	}

	private async Task CheckPOST(ServerModel model)
	{

	}
}
