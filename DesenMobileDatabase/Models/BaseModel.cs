using SQLite;
using System.ComponentModel;

namespace DesenMobileDatabase.Models;

public class BaseModel : INotifyPropertyChanged
{
    [PrimaryKey, AutoIncrement, Column("_id")]
    public int Id { get; set; }

    [MaxLength(250), Unique]
    public string Key { get; set; }

    public DateTime Creation { get; set; }

    public DateTime LastUpdate { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    public BaseModel()
	{
        Creation = DateTime.Now;
        LastUpdate = DateTime.Now;
    }

    public void Updated()
    {
        LastUpdate = DateTime.Now;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
    }

}

