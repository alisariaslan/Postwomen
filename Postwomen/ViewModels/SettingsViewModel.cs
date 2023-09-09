namespace Postwomen.ViewModels;

public class SettingsViewModel : BaseViewModel
{

	public bool IsServiceRunning
	{
		get
		{
#if ANDROID
			return Postwomen.Platforms.Android.AndroidServiceManager.IsRunning;
#else
			return false;
#endif
		}
	}

	public Command StartServiceCommand { get; set; }

	public Command StopServiceCommand { get; set; }

	public SettingsViewModel()
	{
		StartServiceCommand = new Command(StartServiceFunc);
		StopServiceCommand = new Command(StopServiceFunc);
	}

	private async void StartServiceFunc()
	{
#if ANDROID
		Postwomen.Platforms.Android.AndroidServiceManager.StartMyService();
#endif
		await Task.Delay(1000);
		OnPropertyChanged(nameof(IsServiceRunning));
	}

	private async void StopServiceFunc()
	{
#if ANDROID
        Postwomen.Platforms.Android.AndroidServiceManager.StopMyService();
#endif
		await Task.Delay(1000);
		OnPropertyChanged(nameof(IsServiceRunning));
	}



}
