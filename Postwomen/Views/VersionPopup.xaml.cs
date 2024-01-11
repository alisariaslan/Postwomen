using CommunityToolkit.Maui.Views;
using Postwomen.Extensions;
using Postwomen.Interfaces;

namespace Postwomen.Views;

public partial class VersionPopup : Popup
{
	public Translator Translator { get; set; } = new();
	private int currentVersion_ { get; set; }
    public string currentVersion { get { return Translator["current_version"] + ": " + currentVersion_; } }

    private int newVersion_ { get; set; }
    public string newVersion { get { return Translator["new_version"] + ": " + newVersion_; } }

    private IMainApi mainApi { get; set; }

    public VersionPopup(IMainApi mainApi)
    {
        InitializeComponent();
        this.mainApi = mainApi;
        this.BindingContext = this;
    }

    public async Task<bool> CheckVersion()
    {
        try
        {
            var response = await mainApi.GetLatestAppVersion(AppInfo.Current.Name, CancellationToken.None);
            currentVersion_ = AppInfo.Current.Version.Major;
            OnPropertyChanged(nameof(currentVersion));
            if (currentVersion_ < response?.Data?.AndroidVersion)
            {
                newVersion_ = response.Data.AndroidVersion;
                OnPropertyChanged(nameof(newVersion));
                return true;
            }
        }
        catch (Exception)
        {
        }
        return false;
    }

    async void btn_download_Clicked(object sender, EventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://asprojects93.blogspot.com/2023/12/postwomen.html");
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert(Translator["errorOccured"], ex.Message, Translator["ok"]);
        }
    }

    void btn_cancel_Clicked(object sender, EventArgs e)
    {
        this.Close();
    }
}

