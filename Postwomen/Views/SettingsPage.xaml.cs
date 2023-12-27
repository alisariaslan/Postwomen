using Postwomen.Extensions;
using Postwomen.Services;
using System.Globalization;

namespace Postwomen.Views;

[QueryProperty(nameof(BackAction), "BackAction")]
public partial class SettingsPage : ContentPage
{
    private Action BackAction_ { get; set; }
    private int SelectedLangValue_ { get; set; }
    public int SelectedLangValue { get { return SelectedLangValue_; } set { SelectedLangValue_ = value; OnPropertyChanged(nameof(SelectedLangValue)); } }
    public Command SelectLanguageCommand { get; set; }
    public Command ResetLogsCommand { get; set; }
    public Command ResetCardsCommand { get; set; }
    public string AppVersion { get
        {
            return $"{AppInfo.Current.Name}: {AppInfo.Current.VersionString},{Environment.NewLine}" +
               $"{DeviceInfo.Current.Platform}: {DeviceInfo.Current.Version}";
        } }

    public int MaxLogCount
    {
        get { return Preferences.Get("MaxLogCount", 500); }
        set
        {
            if (value < 10)
                return;
            Preferences.Set("MaxLogCount", value);
        }
    }
    public Action BackAction
    {
        get => BackAction_;
        set
        {
            BackAction_ = value;
            OnPropertyChanged(nameof(BackAction));
        }
    }

    private IDbService dbService { get; set; }

    public SettingsPage(IDbService dbService)
    {
        InitializeComponent();
        this.dbService = dbService;
        SelectLanguageCommand = new Command<string>(execute: SelectLanguageFunc);
        ResetLogsCommand = new Command<string>(execute: ResetLogs);
        ResetCardsCommand = new Command<string>(execute: ResetCards);
        var culture = Preferences.Get(nameof(CultureInfo), "en-US");
        if (culture.Equals("tr-TR", StringComparison.OrdinalIgnoreCase))
            SelectedLangValue = 1;
        else
            SelectedLangValue = 0;
        this.BindingContext = this;
    }

    private void SelectLanguageFunc(string param)
    {
        SelectedLangValue = Convert.ToInt32(param);
        switch (SelectedLangValue)
        {
            case 1:
                Preferences.Set(nameof(CultureInfo), "tr-TR");
                break;
            default:
                Preferences.Set(nameof(CultureInfo), "en-US");
                break;
        }
        #region LANGUAGE 
        var culture = Preferences.Get(nameof(CultureInfo), "en - US");
        if (culture.Equals("tr-TR", StringComparison.OrdinalIgnoreCase))
            Translator.Instance.CultureInfo = new CultureInfo("tr-TR");
        else
            Translator.Instance.CultureInfo = new CultureInfo("en-US");
        #endregion
        Translator.Instance.OnPropertyChanged();
    }

    private async void ResetLogs(string param)
    {
        bool answer = await App.Current.MainPage.DisplayAlert(Translator.Instance["resetlogs"], Translator.Instance["resetlogsask"], Translator.Instance["yes"], Translator.Instance["no"]);
        if (answer is false)
            return;
        try
        {
            var result = await dbService.ClearLogs();
            await App.Current.MainPage.DisplayAlert(Translator.Instance["operationresult"], Translator.Instance["deletedCount"] + ": " + result, Translator.Instance["ok"]);
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert(Translator.Instance["errorOccured"], ex.Message, Translator.Instance["ok"]);
        }
        finally { BackAction.Invoke(); }
    }

    private async void ResetCards(string param)
    {
        bool answer = await App.Current.MainPage.DisplayAlert(Translator.Instance["resetservercards"], Translator.Instance["resetservercardsask"], Translator.Instance["yes"], Translator.Instance["no"]);
        if (answer is false)
            return;
        try
        {
            var result = await dbService.ClearCards();
            await App.Current.MainPage.DisplayAlert(Translator.Instance["operationresult"], Translator.Instance["deletedCount"] + ": " + result, Translator.Instance["ok"]);
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert(Translator.Instance["errorOccured"], ex.Message, Translator.Instance["ok"]);
        }
        finally { BackAction.Invoke(); }
    }


}

