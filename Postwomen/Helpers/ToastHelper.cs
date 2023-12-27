using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace Postwomen.Helpers
{
    public class ToastHelper
    {
        public static async void MakeToastFast(string message)
        {
            ToastDuration duration = ToastDuration.Short;
            string text = message;
            var toast = Toast.Make(text, duration, 16);
            await toast.Show(CancellationToken.None);
        }
    }
}
