using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Widget;
using CommunityToolkit.Mvvm.Messaging;
using Postwomen.Platforms.Android;

namespace Postwomen;

[Activity(LaunchMode = LaunchMode.Multiple, Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public static Activity main_act;
    public MainActivity()
    {
        main_act = this;
        var messenger = MauiApplication.Current?.Services?.GetService<IMessenger>();
        messenger?.Register<MessageData>(this, (recipient, message) =>
        {
            if (message.Message.Equals("user") || message.Message.Equals("boot"))
            {
                if (message.Start)
                    StartService(message.Message);
                else
                    StopService();
            }
        });
    }

    public void StartService(string startedFrom)
    {
        var serviceIntent = new Intent(this, typeof(MyNotificationService));
        serviceIntent.PutExtra("startedFrom", startedFrom);
        StartService(serviceIntent);
    }

    public void StopService()
    {
        var serviceIntent = new Intent(this, typeof(MyNotificationService));
        StopService(serviceIntent);
        Toast.MakeText(this, "Postwomen notification service is stopped.", ToastLength.Short).Show();
    }
}
