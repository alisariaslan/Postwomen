using Postwomen.Others;
using SQLite;

namespace Postwomen.Models;

[Table("Servers")]
public class ServerModel
{
	public ServerModel() {
		SaveCardCommand = new Command<int>(execute: SaveCardFunc);
	}

	[PrimaryKey, AutoIncrement, Column("_id")]
	public int Id { get; set; }

	[MaxLength(250)]
	public string Name { get; set; }

	[MaxLength(250)]
	public string Url { get; set; }

	public int Port { get; set; } = 443;

	[MaxLength(250)]
	public string Description { get; set; }

	public bool IsAutoCheckEnabled { get; set; }

	public bool IsSendNotificationsOnChangesEnabled { get; set; }

	public bool IsAdvancedSettingsEnabled { get; set; }

	[Ignore]
	public Command SaveCardCommand { get; set; }

	private async void SaveCardFunc(int param)
	{
		var db = MauiProgram.MyServiceProvider.GetRequiredService<PostwomenDatabase>();
		await db.UpdateItemAsync(this);
	}
}

