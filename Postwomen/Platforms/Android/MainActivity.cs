using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Widget;
using CommunityToolkit.Mvvm.Messaging;
using Postwomen.Platforms.Android;

namespace Postwomen;

[Activity(LaunchMode = LaunchMode.SingleInstance,Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public static Activity main_act;
	public MainActivity()
	{
        main_act = this;
        var messenger = MauiApplication.Current.Services.GetService<IMessenger>();
        messenger.Register<MessageData>(this, (recipient, message) =>
        {
            Toast.MakeText(this, message.ToString(), ToastLength.Short).Show();
            if (message.Start)
            {
                StartService();
            }
            else
            {
                StopService();
            }
        });
    }

	public void StartService()
	{
		var serviceIntent = new Intent(this, typeof(MyNotificationService));
		serviceIntent.PutExtra("inputExtra", "Background Service");
		StartService(serviceIntent);
        Toast.MakeText(this, "Service is started.", ToastLength.Short).Show();
    }

	public void StopService()
	{
		var serviceIntent = new Intent(this, typeof(MyNotificationService));
		StopService(serviceIntent);
        Toast.MakeText(this, "Service is stopped.", ToastLength.Short).Show();
    }
}
