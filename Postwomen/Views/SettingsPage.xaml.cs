using Postwomen.ViewModels;

namespace Postwomen.Views;

public partial class SettingsPage : ContentPage
{
	
	public SettingsPage(IServiceProvider serviceProvider)
	{
		InitializeComponent();
		this.BindingContext = serviceProvider.GetRequiredService<SettingsViewModel>();
        #region THEME
        var style = Preferences.Get("Style", Application.Current.PlatformAppTheme.ToString());
        if (style.Equals(AppTheme.Dark.ToString()))
            checkbox_darkmode.IsChecked = true;
        else
            checkbox_darkmode.IsChecked = false;
        #endregion
    }

    private void checkbox_darkmode_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if(e.Value)
        {
            Preferences.Set("Style", AppTheme.Dark.ToString());
        } else
        {
            Preferences.Set("Style", AppTheme.Light.ToString());
        }
        #region THEME
        var style = Preferences.Get("Style", Application.Current.PlatformAppTheme.ToString());
        if (style.Equals(AppTheme.Dark.ToString()))
            Application.Current.UserAppTheme = AppTheme.Dark;
        else
            Application.Current.UserAppTheme = AppTheme.Light;
        #endregion
    }
}

