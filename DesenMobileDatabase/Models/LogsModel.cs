using DesenMobileDatabase.Enums;
using SQLite;

namespace DesenMobileDatabase.Models;

[Table("Logs")]
public class LogsModel : BaseModel
{
    public LogsTypeEnum Type { get; set; }

    public string Desc { get; set; }

    public LogsModel()
    {

    }

    public LogsModel(LogsTypeEnum type, string desc = "")
    {
        this.Type = type;
        this.Desc = desc;
    }
}
