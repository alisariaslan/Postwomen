using DesenMobileDatabase;
using DesenMobileDatabase.Enums;
using DesenMobileDatabase.Models;
using Postwomen.Extensions;
using Postwomen.Services;
using System.Globalization;

namespace Postwomen;

public partial class App : Application
{
    public App(MobileDB mobileDB,IDbService dbService)
    {

        #region DB
        mobileDB.Init();
        Task.Run(mobileDB.InitAsync);

        dbService.InsertLog(new LogsModel(LogsTypeEnum.AppLaunch,"App started."));
        #endregion

        #region THEME
        Application.Current.UserAppTheme = AppTheme.Light;
        #endregion

        #region LANGUAGE
        var original = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        var culture = Preferences.Get(nameof(CultureInfo), string.Empty);
        if (string.IsNullOrEmpty(culture) && original.Equals("tr"))
        {
            culture = "tr-TR";
            Preferences.Set(nameof(CultureInfo), culture);
        }

        if (culture.Equals("tr-TR", StringComparison.OrdinalIgnoreCase))
            Translator.Instance.CultureInfo = new CultureInfo("tr-TR");
        else
            Translator.Instance.CultureInfo = new CultureInfo("en-US");
        #endregion

        InitializeComponent();
        MainPage = new AppShell();
    }

}
