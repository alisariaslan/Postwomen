using SQLite;

namespace DesenMobileDatabase.Models;

public class BaseModel
{
    [PrimaryKey, AutoIncrement, Column("_id")]
    public int Id { get; set; }

    [MaxLength(250), Unique]
    public string Key { get; set; }

    public DateTime Creation { get; set; }

    public DateTime LastUpdate { get; set; }

    public BaseModel()
	{
        Creation = DateTime.UtcNow;
        LastUpdate = DateTime.UtcNow;
    }

    public void Update() => LastUpdate = DateTime.UtcNow;
        
}

