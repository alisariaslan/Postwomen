using DesenMobileDatabase.Enums;
using SQLite;

namespace DesenMobileDatabase.Models;

[Table("Settings")]
public class SettingsModel : BaseModel
{
    public SettingsTypeEnum Type { get; set; }

    public string Value { get; set; }

    public SettingsModel()
    {

    }

    public SettingsModel(SettingsTypeEnum type, object value)
    {
        this.Type = type;
        this.Value = value?.ToString();
    }
}