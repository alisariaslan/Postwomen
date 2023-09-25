using Android.App;
using Android.OS;
using Android.Runtime;
namespace Postwomen;
[Application]
public class MainApplication : MauiApplication
{
    public static readonly string ChannelName = "General";
    public MainApplication(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership) { }
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    public override void OnCreate()
    {
        base.OnCreate();

        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
#pragma warning disable CA1416
            var channel = new NotificationChannel(ChannelName, "Server Cards", NotificationImportance.High);
            if (GetSystemService(NotificationService) is NotificationManager manager)
                manager.CreateNotificationChannel(channel);
#pragma warning restore CA1416
        }
    }
}
