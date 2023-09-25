using System.Globalization;

namespace Postwomen;

public partial class App : Application
{
    public App()
    {
        #region THEME
        var style = Preferences.Get("Style", Application.Current.PlatformAppTheme.ToString());
        if (style.Equals(AppTheme.Dark.ToString()))
            Application.Current.UserAppTheme = AppTheme.Dark;
        else
            Application.Current.UserAppTheme = AppTheme.Light;
        #endregion

        #region LANGUAGE
        var culture = Preferences.Get("Language", CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
        if (culture.Equals("tr", StringComparison.OrdinalIgnoreCase))
            CultureInfo.CurrentUICulture = new CultureInfo("tr-TR");
        else
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        #endregion

        InitializeComponent();
        MainPage = new AppShell();
    }

    protected override void OnStart()
    {
        base.OnStart();

//#if ANDROID
//        if (!Postwomen.Platforms.Android.AndroidServiceManager.IsRunning)
//			Postwomen.Platforms.Android.AndroidServiceManager.StartMyService();
//#endif

    }

}
