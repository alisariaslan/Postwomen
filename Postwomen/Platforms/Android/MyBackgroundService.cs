using Android.App;
using Android.Content;
using Android.OS;
using Postwomen.Others;

namespace Postwomen.Platforms.Android;

[Service]
public class MyBackgroundService : Service
{
	private Timer Timer { get; set; }
	private ServiceEvents ServiceEvents { get; set; }

	public override IBinder OnBind(Intent intent)
	{
		return null;
	}

	public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
	{
		ServiceEvents = new ServiceEvents();
		Timer = new Timer(Timer_Elapsed, null, 0, 30 * 1000);
		return StartCommandResult.Sticky;
	}

	private void Timer_Elapsed(object state)
	{
		AndroidServiceManager.IsRunning = true;
		ServiceEvents.Checkup();
	}

}