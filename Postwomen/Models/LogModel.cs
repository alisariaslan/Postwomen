using SQLite;

namespace Postwomen.Models;

[Table("Logs")]
public class LogModel
{
	[PrimaryKey, AutoIncrement, Column("_id")]
	public int Id { get; set; }

	public string Description { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.Now;

}

