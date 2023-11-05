using Android.App;
using Android.Content;
using AndroidX.Core.Content;

namespace Postwomen.Platforms.Android;

[BroadcastReceiver(Enabled = true, Exported = true, DirectBootAware = true)]
[IntentFilter(new[] { Intent.ActionBootCompleted })]
public class BootReceiver : BroadcastReceiver
{
    public override async void OnReceive(Context context, Intent intent)
    {
        if (intent.Action == Intent.ActionBootCompleted)
        {
			await Task.Delay(60000);
            var serviceIntent = new Intent(context, typeof(MyNotificationService));
            serviceIntent.PutExtra("startedFrom", "boot");
            ContextCompat.StartForegroundService(context, serviceIntent);
        }
    }
}
