
using DesenMobileDatabase.Enums;
using SQLite;

namespace DesenMobileDatabase.Models;

[Table("ServerCards")]
public class ServerModel : BaseModel
{

	[MaxLength(250)]
	public string Name { get; set; }

	[MaxLength(250)]
	public string Url { get; set; }

	[MaxLength(250)]
	public string Description { get; set; }

	public bool IsAdvancedSettingsEnabled { get; set; }

	public RemoteCallTypes TypeOfCall { get; set; } = RemoteCallTypes.Ping;

    public ProtocolTypes TypeOfProtocol { get; set; } = ProtocolTypes.Https;

    public int Port { get; set; } = 443;

	public CheckStates CurrentState { get; set; } = CheckStates.UNREACHABLE;

	public bool AutoCheck { get; set; } = true;

	[Ignore]
	public Command SaveCardCommand { get; set; }

	public ServerModel()
	{ }

	public ServerModel(string name, string url, string desc, RemoteCallTypes type, int port = 443)
	{
		this.Name = name;
		this.Url = url;
		this.Description = desc;
		this.TypeOfCall = type;
		this.Port = port;
	}

	public bool HasDescription => !string.IsNullOrEmpty(Description);
}

