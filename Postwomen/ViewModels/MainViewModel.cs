using CommunityToolkit.Mvvm.Messaging;
using Postwomen.Models;
using Postwomen.Others;
using Postwomen.Resources.Strings;
using System.Collections.ObjectModel;

namespace Postwomen.ViewModels;

public class MainViewModel : BaseViewModel
{
    private PostwomenDatabase MyPostwomenDatabase { get; set; }
    public ObservableCollection<ServerModel> ServerCards { get; set; } //NULL İKEN DATA ÇAĞIRIR
    public bool HasData { get { if (ServerCards.Count > 0) return true; else return false; } }
    public bool HasDataReversed { get { if (ServerCards.Count > 0) return false; else return true; } }
    public Command RefreshCommand { get; set; }
    public Command LogsCommand { get; set; }
    public Command CreateNewCardCommand { get; set; }
    public Command EditCardCommand { get; set; }
    public Command GoToWebCommand { get; set; }
    public Command CopyCardCommand { get; set; }
    public Command DeleteCardCommand { get; set; }
    public Command SettingsCommand { get; set; }
    private bool IsRefreshing_ { get; set; }
    public bool IsRefreshing { get => IsRefreshing_; set { IsRefreshing_ = value; OnPropertyChanged(nameof(IsRefreshing)); } }
    public ItemsLayout ItemsLayout { get; set; }
    public MainViewModel(IServiceProvider serviceProvider, PostwomenDatabase postwomenDatabase, IMessenger messenger)
    {
        ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical) { ItemSpacing = 1 };
        DeviceDisplay.MainDisplayInfoChanged += OnOriantationChanged;
        MyPostwomenDatabase = postwomenDatabase;
        ServerCards = new ObservableCollection<ServerModel>();
        RefreshCommand = new Command<string>(execute: RefreshList);
        EditCardCommand = new Command<int>(execute: EditCardFunc);
        CopyCardCommand = new Command<int>(execute: CopyCardFunc);
        DeleteCardCommand = new Command<int>(execute: DeleteCardFunc);
        CreateNewCardCommand = new Command<string>(execute: CreateNewCardFunc);
        GoToWebCommand = new Command<string>(execute: GoToWebFunc);
        SettingsCommand = new Command(GoToSettingsFunc);
        LogsCommand = new Command(GoToLogsFunc);
        RefreshList("");
        messenger.Send(new MessageData("start", true));
    }

    private void OnOriantationChanged(object sender, DisplayInfoChangedEventArgs e)
    {
        switch (e.DisplayInfo.Orientation)
        {
            case DisplayOrientation.Unknown:
                ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical) { ItemSpacing = 1 };
                break;
            case DisplayOrientation.Portrait:
                ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical) { ItemSpacing = 1 };
                break;
            case DisplayOrientation.Landscape:
                ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical) { ItemSpacing = 2 };
                break;
        }
        OnPropertyChanged(nameof(ItemsLayout));
    }

    private async void RefreshList(string param)
    {
        await Task.Delay(100);
        var items = await MyPostwomenDatabase.GetItemsAsync<ServerModel>();
        ServerCards.Clear();
        foreach (var item in items)
            ServerCards.Add(item);
        OnPropertyChanged(nameof(ServerCards));
        OnPropertyChanged(nameof(HasData));
        OnPropertyChanged(nameof(HasDataReversed));
        IsRefreshing = false;
    }
    public async void RefreshListFromService(string param)
    {
        ServerCards.Clear();
        ServerCards = new ObservableCollection<ServerModel>();
        var items = await MyPostwomenDatabase.GetItemsAsync<ServerModel>();
        foreach (var item in items)
            ServerCards.Add(item);
        OnPropertyChanged(nameof(ServerCards));
    }
    private async void GoToSettingsFunc()
    {
        await Shell.Current.GoToAsync($"SettingsPage");
    }
    private async void GoToLogsFunc()
    {
        //await Shell.Current.GoToAsync($"LogsPage");
#if DEBUG
        await Shell.Current.GoToAsync($"ServiceTestPage");
#endif
    }
    private async void GoToWebFunc(string param)
    {
        Console.WriteLine("LOG PARAMETER: " + param);
        bool yes = await App.Current.MainPage.DisplayAlert(AppResources.openbrowser, $"{AppResources.openbrowserask} {param} ?", AppResources.yes, AppResources.no);
        if (yes)
        {
            try
            {
                Uri uri = new Uri(param);
                await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.error, AppResources.maybenointernet + ex, AppResources.ok);
            }
        }
    }
    private async void CreateNewCardFunc(string param)
    {
        Console.WriteLine("LOG PARAMETER: " + param);
        var backAction = new Action(() => RefreshList(""));
        var navigationParameters = new Dictionary<string, object> {
            { "BackAction", backAction },
            { "SelectedCard", null },
            { "CardState", 0 } };
        await Shell.Current.GoToAsync($"EditCardPage", navigationParameters);
    }
    private async void CopyCardFunc(int param)
    {
        var card = await MyPostwomenDatabase.GetServerCard(param);
        Console.WriteLine("LOG PARAMETER: " + param);
        var backAction = new Action(() => RefreshList(""));
        var navigationParameters = new Dictionary<string, object> {
            {"BackAction", backAction},
            {"SelectedCard", card},
            {"CardState",1}
        };
        await Shell.Current.GoToAsync($"EditCardPage", navigationParameters);
    }
    private async void EditCardFunc(int param)
    {
        var card = await MyPostwomenDatabase.GetServerCard(param);
        Console.WriteLine("LOG PARAMETER: " + param);
        var backAction = new Action(() => RefreshList(""));
        var navigationParameters = new Dictionary<string, object> {
            {"BackAction", backAction},
            {"SelectedCard", card},
            {"CardState",2}
        };
        await Shell.Current.GoToAsync($"EditCardPage", navigationParameters);
    }
    private async void DeleteCardFunc(int param)
    {
        bool result = await App.Current.MainPage.DisplayAlert(AppResources.deletingcard, AppResources.deletingcardask, AppResources.yes, AppResources.no);
        if (result is false)
            return;

        var card = await MyPostwomenDatabase.GetServerCard(param);
        await MyPostwomenDatabase.DeleteItemAsync(card);
        RefreshList(string.Empty);
    }
}
