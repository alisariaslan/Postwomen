using CommunityToolkit.Maui.Views;
using Postwomen.Extensions;
using Postwomen.Services;
using System.Globalization;

namespace Postwomen.Views;

[QueryProperty(nameof(BackAction), "BackAction")]
public partial class SettingsPage : ContentPage
{
	public Translator Translator { get; set; }
	private Action BackAction_ { get; set; }
    private int SelectedLangValue_ { get; set; }
    public int SelectedLangValue { get { return SelectedLangValue_; } set { SelectedLangValue_ = value; OnPropertyChanged(nameof(SelectedLangValue)); } }
    public Command SelectLanguageCommand { get; set; }
    public Command ResetLogsCommand { get; set; }
    public Command ResetCardsCommand { get; set; }
    public string AppVersion
    {
        get
        {
            return $"{AppInfo.Current.Name}: {AppInfo.Current.VersionString},{Environment.NewLine}" +
               $"{DeviceInfo.Current.Platform}: {DeviceInfo.Current.Version}";
        }
    }

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
    private IServiceProvider serviceProvider { get; set; }
    public SettingsPage(IDbService dbService, IServiceProvider serviceProvider, Translator translator)
    {
        InitializeComponent();
		this.Translator = translator;
		this.dbService = dbService;
        this.serviceProvider = serviceProvider;
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
            Translator.CultureInfo = new CultureInfo("tr-TR");
        else
            Translator.CultureInfo = new CultureInfo("en-US");
        #endregion
        Translator.OnPropertyChanged();
    }

    private async void ResetLogs(string param)
    {
        bool answer = await App.Current.MainPage.DisplayAlert(Translator["resetlogs"], Translator["resetlogsask"], Translator["yes"], Translator["no"]);
        if (answer is false)
            return;
        try
        {
            var result = await dbService.ClearLogs();
            await App.Current.MainPage.DisplayAlert(Translator["operationresult"], Translator["deletedCount"] + ": " + result, Translator["ok"]);
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert(Translator["errorOccured"], ex.Message, Translator["ok"]);
        }
        finally { BackAction.Invoke(); }
    }

    private async void ResetCards(string param)
    {
        bool answer = await App.Current.MainPage.DisplayAlert(Translator["resetservercards"], Translator["resetservercardsask"], Translator["yes"], Translator["no"]);
        if (answer is false)
            return;
        try
        {
            var result = await dbService.ClearCards();
            await App.Current.MainPage.DisplayAlert(Translator["operationresult"], Translator["deletedCount"] + ": " + result, Translator["ok"]);
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert(Translator["errorOccured"], ex.Message, Translator["ok"]);
        }
        finally { BackAction.Invoke(); }
    }

    async void btn_version_Clicked(object sender, EventArgs e)
    {
        (sender as Button).IsEnabled = false;
        var popup = serviceProvider.GetService<VersionPopup>();
        var upgradeAvaible = await popup.CheckVersion();
        if (upgradeAvaible)
            await this.ShowPopupAsync(popup);
        await Task.Delay(10000);
        (sender as Button).IsEnabled = true;
        if(upgradeAvaible is false)
            await App.Current.MainPage.DisplayAlert(Translator["info"], Translator["updated"], Translator["ok"]);
    }
}

