using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Postwomen.Platforms.Android;

namespace Postwomen;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
	private Intent ServiceIntent { get; set; }

	public MainActivity()
	{
		AndroidServiceManager.MainActivity = this;
	}

	public void StartService()
	{
		if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.PostNotifications) != Permission.Granted)
		{
			ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.PostNotifications }, 0);
		}
		ServiceIntent = new Intent(this, typeof(MyBackgroundService));
		ServiceIntent.PutExtra("inputExtra", "Background Service");
		StartService(ServiceIntent);
	}

	public void StopService()
	{
		StopService(ServiceIntent);
	}
}
