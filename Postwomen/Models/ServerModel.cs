using SQLite;

namespace Postwomen.Models;

[Table("Servers")]
public class ServerModel
{

	[PrimaryKey, AutoIncrement, Column("_id")]
	public int Id { get; set; }

	[MaxLength(250)]
	public string Name { get; set; }

	[MaxLength(250)]
	public string Url{ get; set; }

	public int Port { get; set; } = 443;

	[MaxLength(250)]
	public string Description { get; set; }

	public bool IsAdvancedSettingsEnabled { get; set; }


}

