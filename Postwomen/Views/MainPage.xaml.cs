using DesenMobileDatabase.Enums;
using DesenMobileDatabase.Models;
using Postwomen.Extensions;
using Postwomen.Helpers;
using Postwomen.Services;
using System.Collections.ObjectModel;

namespace Postwomen.Views;

public partial class MainPage : ContentPage
{
    private IDbService dbService { get; set; }
    public ObservableCollection<ServerModel> ServerCards { get; set; }
    public bool HasData { get { if (ServerCards.Count > 0) return true; else return false; } }
    public bool HasDataReversed { get { if (ServerCards.Count > 0) return false; else return true; } }
    public Command RefreshCommand { get; set; }
    public Command RefreshCardCommand { get; set; }
    public Command LogsCommand { get; set; }
    public Command CreateNewCardCommand { get; set; }
    public Command EditCardCommand { get; set; }
    public Command GoToWebCommand { get; set; }
    public Command CopyCardCommand { get; set; }
    public Command DeleteCardCommand { get; set; }
    public Command SettingsCommand { get; set; }
    private bool IsRefreshing_ { get; set; }
    public bool IsRefreshing { get => IsRefreshing_; set { IsRefreshing_ = value; OnPropertyChanged(nameof(IsRefreshing)); } }

    public MainPage(IDbService dbService)
    {
        InitializeComponent();
        this.dbService = dbService;
        ServerCards = new ObservableCollection<ServerModel>();
        RefreshCommand = new Command(execute: RefreshCards);
        RefreshCardCommand = new Command<int>(execute: RefreshCard);
        EditCardCommand = new Command<int>(execute: EditCardFunc);
        CopyCardCommand = new Command<int>(execute: CopyCardFunc);
        DeleteCardCommand = new Command<int>(execute: DeleteCardFunc);
        CreateNewCardCommand = new Command<string>(execute: CreateNewCardFunc);
        GoToWebCommand = new Command<int>(execute: GoToWebFunc);
        SettingsCommand = new Command(GoToSettingsFunc);
        LogsCommand = new Command(GoToLogsFunc);
        this.BindingContext = this;
    }

    async void CollectionView_Loaded(object sender, EventArgs e)
    {
        RefreshCards();
        refreshButton.IsEnabled = false;
        while (IsRefreshing)
            await refreshButton.RelRotateTo(360, 1000);
        refreshButton.IsEnabled = true;
    }

    async void ImageButton_Clicked(object sender, EventArgs e)
    {
        var obj = sender as VisualElement;
        obj.IsEnabled = false;
        while (IsRefreshing)
            await obj.RelRotateTo(360, 1000);
        obj.IsEnabled = true;
    }

    private async void RefreshCard(int param)
    {
        if (IsRefreshing)
            return;
        IsRefreshing = true;
        var selected = ServerCards.FirstOrDefault(card => card.Id == param);
        await CheckCard(selected);
        IsRefreshing = false;
    }

    private void ForceRefresh()
    {
        ServerCards.Clear();
        IsRefreshing = false;
        RefreshCards();
    }

    private async void RefreshCards()
    {
        if (IsRefreshing)
            return;
        IsRefreshing = true;
        dbService.InsertLog(new LogsModel(LogsTypeEnum.General, "Card list refresh toggled."));
        ServerCards.Clear();
        var items = await dbService.GetCards();
        items.ForEach(ServerCards.Add);
        OnPropertyChanged(nameof(HasData));
        OnPropertyChanged(nameof(HasDataReversed));
        var tasklist = new List<Task>();
        await Task.Delay(1000);
        foreach (var item in ServerCards)
            tasklist.Add(CheckCard(item));
        await Task.WhenAll(tasklist.ToArray());
        IsRefreshing = false;
    }

    private async Task CheckCard(ServerModel model)
    {
        if (model.CurrentState != CheckStates.TRYING_TO_REACH)
        {
            dbService.InsertLog(new LogsModel(LogsTypeEnum.General, $"{model.Name}: Card is refreshing..."));
            model.CurrentState = CheckStates.TRYING_TO_REACH;
            model.Updated();
            await Task.Delay(1000);
            PingHelper pingHelper = new PingHelper(dbService);
            bool success = await pingHelper.CheckPing(model.Url);
            if (success)
            {
                dbService.InsertLog(new LogsModel(LogsTypeEnum.General, $"{model.Name}: Card is reachable."));
                model.CurrentState = CheckStates.REACHABLE;
            }
            else
            {
                dbService.InsertLog(new LogsModel(LogsTypeEnum.General, $"{model.Name}: Card is unreachable!"));
                model.CurrentState = CheckStates.UNREACHABLE;
            }
            model.Updated();
        } else
        {
            dbService.InsertLog(new LogsModel(LogsTypeEnum.General, $"{model.Name}: Card is already refreshing..."));
        }
    }

    private async void GoToSettingsFunc()
    {
        var backAction = new Action(() => ForceRefresh());
        var navigationParameters = new Dictionary<string, object> {
            {"BackAction", backAction}
        };
        await Shell.Current.GoToAsync($"SettingsPage", navigationParameters);
    }
    private async void GoToLogsFunc()
    {
        await Shell.Current.GoToAsync($"LogsPage");
    }
    private async void GoToWebFunc(int param)
    {
        var card = ServerCards.FirstOrDefault(c => c.Id.Equals(param));
        var newUri = card.TypeOfCall is RemoteCallTypes.Ping ? "http://"+card.Url : card.Url;

        bool yes = await App.Current.MainPage.DisplayAlert(Translator.Instance["openbrowser"], $"{Translator.Instance["openbrowserask"]} {card.Name} ?", Translator.Instance["yes"], Translator.Instance["no"]);
        if (yes)
        {
            try
            {
                Uri uri = new Uri(newUri);
                await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Translator.Instance["errorOccured"], ex.Message, Translator.Instance["ok"]);
            }
        }
    }
    private async void CreateNewCardFunc(string param)
    {
        var backAction = new Action(() => ForceRefresh());
        var navigationParameters = new Dictionary<string, object> {
            { "BackAction", backAction },
            { "SelectedCard", null },
            { "CardState", 0 } };
        await Shell.Current.GoToAsync($"EditCardPage", navigationParameters);
    }
    private async void CopyCardFunc(int param)
    {
        var card = ServerCards.FirstOrDefault(c => c.Id.Equals(param));
        var backAction = new Action(() => ForceRefresh());
        var navigationParameters = new Dictionary<string, object> {
            {"BackAction", backAction},
            {"SelectedCard", card},
            {"CardState",1}
        };
        await Shell.Current.GoToAsync($"EditCardPage", navigationParameters);
    }
    private async void EditCardFunc(int param)
    {
        var card = ServerCards.FirstOrDefault(c => c.Id.Equals(param));
        var backAction = new Action(() => ForceRefresh());
        var navigationParameters = new Dictionary<string, object> {
            {"BackAction", backAction},
            {"SelectedCard", card},
            {"CardState",2}
        };
        await Shell.Current.GoToAsync($"EditCardPage", navigationParameters);
    }
    private async void DeleteCardFunc(int param)
    {
        bool result = await App.Current.MainPage.DisplayAlert(Translator.Instance["deletingcard"], Translator.Instance["deletingcardask"], Translator.Instance["yes"], Translator.Instance["no"]);
        if (result is false)
            return;

        var card = ServerCards.FirstOrDefault(c => c.Id.Equals(param));
        try
        {
            await dbService.DelCard(card);
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert(Translator.Instance["errorOccured"], ex.Message, Translator.Instance["ok"]);
        }
        ForceRefresh();
    }

}

