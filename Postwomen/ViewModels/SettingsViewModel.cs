using Postwomen.Enums;
using Postwomen.Models;
using Postwomen.Others;
using Postwomen.Resources.Strings;
using System.Globalization;

namespace Postwomen.ViewModels;

public class SettingsViewModel : BaseViewModel
{
    private PostwomenDatabase MyPostwomenDatabase { get; set; }
    private int SelectedLangValue_ { get; set; }
    public int SelectedLangValue { get { return SelectedLangValue_; } set { SelectedLangValue_ = value; OnPropertyChanged(nameof(SelectedLangValue)); } }
    public Command SelectLanguageCommand { get; set; }
    public Command ResetDBCommand { get; set; }

    public int CheckCycleValue
    {
        get { return Preferences.Get("CheckCycleValue", 600); }
        set
        {
            if (value < 30)
                return;
            Preferences.Set("CheckCycleValue", value);
        }
    }

    public int MaxLogCount
    {
        get { return Preferences.Get("MaxLogCount", 5000); }
        set
        {
            if (value < 10)
                return;
            Preferences.Set("MaxLogCount", value);
        }
    }

    public SettingsViewModel(PostwomenDatabase postwomenDatabase)
    {
        SelectLanguageCommand = new Command<string>(execute : SelectLanguageFunc);
        ResetDBCommand = new Command<string>(execute: ResetDB);
        this.MyPostwomenDatabase = postwomenDatabase;
        var culture = Preferences.Get("Language", CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
        if (culture.Equals("tr", StringComparison.OrdinalIgnoreCase))
            SelectedLangValue = 1;
        else
            SelectedLangValue = 0;
    }

    private async void SelectLanguageFunc(string param)
    {
        SelectedLangValue = Convert.ToInt32(param);
        switch (SelectedLangValue)
        {
            case 1:
                Preferences.Set("Language", new CultureInfo("tr-TR").TwoLetterISOLanguageName);
                break;
            default:
                Preferences.Set("Language", new CultureInfo("en-US").TwoLetterISOLanguageName);
                break;
        }
        #region LANGUAGE
        var culture = Preferences.Get("Language", CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
        if (culture.Equals("tr", StringComparison.OrdinalIgnoreCase))
            CultureInfo.CurrentUICulture = new CultureInfo("tr-TR");
        else
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        #endregion
        await App.Current.MainPage.DisplayAlert(AppResources.warning, AppResources.appclosing, AppResources.ok);
        Application.Current.Quit();
    }


    private async void ResetDB(string param)
    {
        bool answer = await App.Current.MainPage.DisplayAlert(AppResources.resetdatabase, AppResources.resetdatabaseask, AppResources.yes, AppResources.no);
        if (answer is false)
            return;

        try
        {
            bool deleteServerCards = await App.Current.MainPage.DisplayAlert(AppResources.resetservercards, AppResources.resetservercardsask, AppResources.yes, AppResources.no);
            if (deleteServerCards is true)
            {
                var result = await MyPostwomenDatabase.DropTableAsync<ServerModel>();
                await App.Current.MainPage.DisplayAlert(AppResources.operationresult, OperationStatesLangConverter.Convert(result), AppResources.ok);
            }

            bool deleteLogs = await App.Current.MainPage.DisplayAlert(AppResources.resetlogs, AppResources.resetlogsask, AppResources.yes, AppResources.no);
            if (deleteLogs is true)
            {
                Preferences.Set("LogCount", 0);
                var result = await MyPostwomenDatabase.DropTableAsync<LogModel>();
                await App.Current.MainPage.DisplayAlert(AppResources.operationresult, ((OperationStates)result).ToString(), AppResources.ok);
            }

        }
        catch (Exception e)
        {
            await App.Current.MainPage.DisplayAlert(AppResources.error, e.Message, AppResources.ok);
        }
        finally
        {
            await App.Current.MainPage.DisplayAlert(AppResources.warning, AppResources.appclosing, AppResources.ok);
            Application.Current.Quit();
        }

    }
}
