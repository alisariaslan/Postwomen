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

	public int TypeOfCall { get; set; } = 1;

	public int Port { get; set; } = 443;

	public bool IsAutoCheckEnabled { get; set; }

	public bool IsSendNotificationsOnChangesEnabled { get; set; }

	public bool IsSeperatedFromGeneralNotifications { get; set; }

	public int CurrentState { get; set; }

	[Ignore]
	public Command SaveCardCommand { get; set; }

	[Ignore]
	public bool HasDescription { get { if (Description == null || Description.Equals("")) return false; else return true; } }

	[Ignore]
	public string PortString
	{
		get
		{
			if (TypeOfCall == 1) return "Ping only";
			else if (TypeOfCall == 2) return $"GET({Port})";
			else if (TypeOfCall == 3) return $"POST({Port})";
			else return "Unkown";
		}
	}

	private async void SaveCardFunc(int param)
	{
		var db = new PostwomenDatabase();
		await db.UpdateItemAsync(this);
	}
}

