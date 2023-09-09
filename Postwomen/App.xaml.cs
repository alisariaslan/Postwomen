namespace Postwomen;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		MainPage = new AppShell();
	}

	protected override void OnStart()
	{
		base.OnStart();

#if ANDROID
        if (!Postwomen.Platforms.Android.AndroidServiceManager.IsRunning)
			Postwomen.Platforms.Android.AndroidServiceManager.StartMyService();
#endif
	}


}
