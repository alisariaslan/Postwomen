using SQLite;

namespace Postwomen;

public class ServerModel
{
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }
	public string Name { get; set; }
	public string Url { get; set; }
	public string Description { get; set; }
	public bool HasDescription { get { if (Description == null || Description.Equals("")) return false; else return true; } }
	public int Port { get; set; }
	public int State { get; set; } //0, 1, 2
	public Color StateColor { get { if (State == 0) return Colors.Red; else if (State == 1) return Colors.Orange; else return Colors.Green; } }
	public bool AutoCheckEnabled { get; set; }
	public bool NotificationEnabled { get; set; }

}

