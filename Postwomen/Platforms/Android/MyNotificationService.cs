using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using CommunityToolkit.Mvvm.Messaging;
using Postwomen.Enums;
using Postwomen.Models;
using Postwomen.Others;
using Postwomen.Resources.Strings;

namespace Postwomen.Platforms.Android;

[Service]
public class MyNotificationService : Service
{
    private int badgeNumber;
    private PostwomenDatabase pwDatabase;
    private Timer timer { get; set; }
    private bool isEnterable = false;
    public static bool isTimerActive = false;

    public override IBinder OnBind(Intent intent)
    {

        return null;
    }

    public override bool OnUnbind(Intent intent)
    {

        return base.OnUnbind(intent);
    }

    public static void RequestNotifPerm()
    {
        if (MainActivity.main_act is not null && ContextCompat.CheckSelfPermission(MainActivity.main_act, Manifest.Permission.PostNotifications) != Permission.Granted)
            ActivityCompat.RequestPermissions(MainActivity.main_act, new[] { Manifest.Permission.PostNotifications }, 0);
    }

    private NotificationCompat.Builder CreateNewNotification(PendingIntent pendingIntent)
    {
        return new NotificationCompat.Builder(this, MainApplication.ChannelName)
              .SetSmallIcon(Resource.Drawable.postwomen_circle_notification)
              .SetContentIntent(pendingIntent)
              .SetContentTitle(AppResources.serversarenotreachable)
              .SetChannelId(MainApplication.ChannelName)
              .SetAutoCancel(true)
              .SetOngoing(false);
    }

    public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
    {
        var startedFrom = intent.GetStringExtra("startedFrom");
        Toast.MakeText(this, "Service is started from " + startedFrom, ToastLength.Long).Show();
        var notif_intent = new Intent(this, typeof(MainActivity));
        var i = (int)Build.VERSION.SdkInt;
        PendingIntentFlags pendingIntentFlags = PendingIntentFlags.Immutable;
        if ((int)Build.VERSION.SdkInt < 31)
            pendingIntentFlags = PendingIntentFlags.CancelCurrent;
        var pend_intent = PendingIntent.GetActivity(this, 0, notif_intent, pendingIntentFlags);
        pwDatabase = new PostwomenDatabase();
        var cycle = Preferences.Get("CheckCycleValue", 600);
        timer = new Timer(Timer_Elapsed, pend_intent, 0, cycle * 1000);
        new Task(async () => { await Task.Delay(3000); isEnterable = true; }).Start();
        return StartCommandResult.Sticky;
    }

    private async void Timer_Elapsed(object param)
    {
        isTimerActive = true;
        if (isEnterable is false)
            return;

        isEnterable = false;
        var dbItems = await pwDatabase.GetItemsAsync<ServerModel>();
        string notificationBody = string.Empty;
        foreach (var item in dbItems)
        {
            if (item.IsAutoCheckEnabled is false)
                continue;
            bool isOkey = false;
            switch (item.TypeOfCall)
            {
                case 0: isOkey = await CheckPing(item.Url); break;
                    //case 1: await CheckGET(item); break;
                    //case 2: await CheckPOST(item); break;
            }
            if (isOkey)
                item.CurrentState = (int)CheckStates.REACHABLE;
            else
            {
                item.CurrentState = (int)CheckStates.UNREACHABLE;
                string result = $"{item.Name}, {item.Url}: {AppResources.isnotreachable}";
                await pwDatabase.SaveLogAsync(result);
                if (item.IsSendNotificationsOnChangesEnabled)
                {
                    if (item.LastCheck.AddSeconds(item.DelayAfterNotification) <= DateTime.Now)
                    {
                        item.LastCheck = DateTime.Now;
                        await pwDatabase.SaveServerCardAsync(item);
                        if (notificationBody.Equals(string.Empty))
                            notificationBody += result;
                        else notificationBody += "\n" + result;
                    }
                }
            }
        }
        if (notificationBody != string.Empty)
        {
            using (NotificationCompat.Builder notifBuilder = CreateNewNotification((PendingIntent)param))
            {
                notifBuilder.SetContentText(notificationBody).SetNumber(badgeNumber++);
                StartForeground(101010, notifBuilder.Build());
                notifBuilder.Dispose();
            }
        }
        var messenger = MauiApplication.Current?.Services?.GetService<IMessenger>();
        messenger?.Send(new MessageData("service", false));
        var cycle = Preferences.Get("CheckCycleValue", 600);
        timer.Change(0, cycle * 1000);
        new Task(async () => { await Task.Delay(3000); isEnterable = true; }).Start();
    }

    public static async Task<bool> CheckPing(string url)
    {
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
                    return true;
            }
            return false;
        }
    }


}