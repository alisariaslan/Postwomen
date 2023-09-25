using Microsoft.VisualBasic;
using Postwomen.Enums;
using Postwomen.Others;
using SQLite;

namespace Postwomen.Models;

[Table("Servers")]
public class ServerModel
{
	public ServerModel()
	{
		SaveCardCommand = new Command<int>(execute: SaveCardFunc);
	}

	[PrimaryKey, AutoIncrement, Column("_id")]
	public int Id { get; set; }

	[MaxLength(250)]
	public string Name { get; set; }

	[MaxLength(250)]
	public string Url { get; set; }

	[MaxLength(250)]
	public string Description { get; set; }

	public bool IsAdvancedSettingsEnabled { get; set; }

	public int TypeOfCall { get; set; }

	public int Port { get; set; } = 443;

	public bool IsAutoCheckEnabled { get; set; }

	public bool IsSendNotificationsOnChangesEnabled { get; set; }

    public int DelayAfterNotification { get; set; } = 100;

    public int CurrentState { get; set; } = 2;

    public DateTime LastCheck { get; set; } = DateTime.Now.AddDays(-1);

	[Ignore]
	public Command SaveCardCommand { get; set; }

	[Ignore]
	public bool HasDescription { get { if (Description == null || Description.Equals("")) return false; else return true; } }

	[Ignore]
	public string PortString { get { return ((RemoteCallTypes)TypeOfCall).ToString(); } }

	private async void SaveCardFunc(int param)
	{
#if ANDROID
        if (IsSendNotificationsOnChangesEnabled)
            Platforms.Android.MyNotificationService.RequestNotifPerm();
#endif
        var db = new PostwomenDatabase();
		await db.UpdateItemAsync(this);
	}
}

