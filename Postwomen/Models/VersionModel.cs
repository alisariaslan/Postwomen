using DesenMobileDatabase.Models;

namespace Postwomen.Models;

public class VersionModel : ApiModel
{
    public new VersionModelData Data { get; set; }
}

public class VersionModelData : BaseModel
{
    public int AndroidVersion { get; set; }

    public int IOSVersion { get; set; }

    public int WindowsVersion { get; set; }

    public int MacosVersion { get; set; }



}

