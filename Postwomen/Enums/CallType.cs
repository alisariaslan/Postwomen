namespace Postwomen.Enums;

public enum CallType
{
	GET,
	POST
}

public static class GetCallType
{
	public static string Get(int type)
	{
			if (type == 1) return "Ping only";
			else if (type == 2) return $"GET";
			else if (type == 3) return $"POST";
			else return "Unkown";
	}
}
