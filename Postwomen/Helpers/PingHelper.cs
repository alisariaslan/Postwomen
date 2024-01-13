using Android.Icu.Util;
using DesenMobileDatabase.Enums;
using DesenMobileDatabase.Models;
using Postwomen.Services;

namespace Postwomen.Helpers;

public class PingHelper
{
	private IDbService dbService;

	public PingHelper(IDbService dbService)
	{
		this.dbService = dbService;
	}

	public async Task<bool> CheckPing(string url)
	{
		bool result = false;

		try
		{
#if ANDROID
			Java.Lang.Process p1 = Java.Lang.Runtime.GetRuntime().Exec("ping -c 1 " + url);
			int returnVal = await p1.WaitForAsync();
			if (returnVal == 1)
				result = true;
#endif

			using (System.Diagnostics.Process process = new System.Diagnostics.Process())
			{
				process.StartInfo.FileName = "ping";
				process.StartInfo.Arguments = $"-c 1 {url}";
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.CreateNoWindow = true;
				process.Start();
				string output = await process.StandardOutput.ReadToEndAsync();
				process.WaitForExit();
				if (process.ExitCode == 0)
				{
					if (output.Contains("1 packets transmitted, 1 received"))
						result = true;
				}
			}
		}
		catch (Exception ex)
		{
			dbService.InsertLog(new LogsModel(LogsTypeEnum.General, $"{url}: Error occured! Message: " + ex.Message));
			ToastHelper.MakeToastFast("Some errors occured. Please check your logs.");
		}

		return result;
	}
}
