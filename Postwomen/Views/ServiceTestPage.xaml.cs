
using CommunityToolkit.Mvvm.Messaging;

namespace Postwomen.Views;

public partial class ServiceTestPage : ContentPage
{
    private readonly IMessenger _messenger;

    public ServiceTestPage(IServiceProvider serviceProvider, IMessenger messenger)
	{
		InitializeComponent();
        this.BindingContext = this;
        _messenger = messenger;
    }

    private void btn_start_Clicked(object sender, EventArgs e)
    {
        _messenger.Send(new MessageData("start", true));
    }

    private void btn_stop_Clicked(object sender, EventArgs e)
    {
        _messenger.Send(new MessageData("stop", false));
    }

    private void btn_check_Clicked(object sender, EventArgs e)
    {
#if ANDROID
        var state = Platforms.Android.MyNotificationService.isTimerActive;
        App.Current.MainPage.DisplayAlert("Service state",state.ToString() , "ok");
#endif
    }

    private void btn_enable_Clicked(object sender, EventArgs e)
    {

    }

    private void btn_disable_Clicked(object sender, EventArgs e)
    {

    }
}

