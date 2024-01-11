using CommunityToolkit.Maui.Views;
using DesenMobileDatabase.Enums;
using DesenMobileDatabase.Models;
using Postwomen.Extensions;
using Postwomen.Helpers;
using Postwomen.Interfaces;
using Postwomen.Services;
using System.Collections.ObjectModel;

namespace Postwomen.Views;


public partial class MainPage : ContentPage
{
	public Translator Translator { get; set; }
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
	public Command SaveCardCommand { get; set; }
	public Command CopyCardCommand { get; set; }
	public Command DeleteCardCommand { get; set; }
	public Command SettingsCommand { get; set; }
	private bool IsRefreshing_ { get; set; }
	public bool IsRefreshing { get => IsRefreshing_; set { IsRefreshing_ = value; OnPropertyChanged(nameof(IsRefreshing)); } }
	private VersionPopup versionPopup { get; set; }
	public MainPage(IDbService dbService, VersionPopup versionPopup, Translator translator)
	{
		InitializeComponent();
		this.Translator = translator;
		this.dbService = dbService;
		this.versionPopup = versionPopup;
		ServerCards = new ObservableCollection<ServerModel>();
		RefreshCommand = new Command(execute: RefreshCards);
		RefreshCardCommand = new Command<int>(execute: RefreshCard);
		EditCardCommand = new Command<int>(execute: EditCardFunc);
		SaveCardCommand = new Command<int>(execute: SaveCardFunc);
		CopyCardCommand = new Command<int>(execute: CopyCardFunc);
		DeleteCardCommand = new Command<int>(execute: DeleteCardFunc);
		CreateNewCardCommand = new Command<string>(execute: CreateNewCardFunc);
		GoToWebCommand = new Command<string>(execute: GoToWebFunc);
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
		items.ForEach(i =>
		{
			i.SaveCardCommand = new Command<int>(SaveCardFunc);
			ServerCards.Add(i);
		});
		OnPropertyChanged(nameof(HasData));
		OnPropertyChanged(nameof(HasDataReversed));
		var tasklist = new List<Task>();
		await Task.Delay(1000);
		foreach (var item in ServerCards)
		{
			if (item.AutoCheck)
				tasklist.Add(CheckCard(item));
		}
		await Task.WhenAll(tasklist.ToArray());
		IsRefreshing = false;
	}

	private async Task CheckCard(ServerModel model)
	{
		try
		{
			if (model.CurrentState != CheckStates.TRYING_TO_REACH)
			{
				dbService.InsertLog(new LogsModel(LogsTypeEnum.General, $"{model.Name}: Card is refreshing..."));
				model.CurrentState = CheckStates.TRYING_TO_REACH;
				model.Updated();
				await Task.Delay(1000);
				if (model.TypeOfCall == RemoteCallTypes.GET)
				{
				}
				else if (model.TypeOfCall == RemoteCallTypes.POST)
				{
				}
				else if (model.TypeOfCall == RemoteCallTypes.INSERT)
				{
				}
				else if (model.TypeOfCall == RemoteCallTypes.UPDATE)
				{
				}
				else if (model.TypeOfCall == RemoteCallTypes.DELETE)
				{
				}
				else
				{
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
				}
			}
			else
			{
				dbService.InsertLog(new LogsModel(LogsTypeEnum.General, $"{model.Name}: Card is already refreshing..."));
			}
		}
		catch (Exception ex)
		{
			dbService.InsertLog(new LogsModel(LogsTypeEnum.Error, $"'{model.Name}' -> Error occured when refresh. Error message: {ex}"));
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
	private async void GoToWebFunc(string param)
	{
		string newUri = string.Empty;
		if (param.Contains("http") is false)
			newUri = "http://" + param;
		else newUri = param;

		bool yes = await App.Current.MainPage.DisplayAlert(Translator["openbrowser"], $"{Translator["openbrowserask"]} {param} ?", Translator["yes"], Translator["no"]);
		if (yes)
		{
			try
			{
				Uri uri = new Uri(newUri);
				await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
			}
			catch (Exception ex)
			{
				await App.Current.MainPage.DisplayAlert(Translator["errorOccured"], ex.Message, Translator["ok"]);
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

	private async void SaveCardFunc(int param)
	{
		var card = ServerCards.FirstOrDefault(c => c.Id.Equals(param));
		var resultString = await dbService.UpdateCard(card) ? Translator["changed"] : Translator["fail"];
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
		bool result = await App.Current.MainPage.DisplayAlert(Translator["deletingcard"], Translator["deletingcardask"], Translator["yes"], Translator["no"]);
		if (result is false)
			return;

		var card = ServerCards.FirstOrDefault(c => c.Id.Equals(param));
		try
		{
			await dbService.DelCard(card);
		}
		catch (Exception ex)
		{
			await App.Current.MainPage.DisplayAlert(Translator["errorOccured"], ex.Message, Translator["ok"]);
		}
		ForceRefresh();
	}

	async void ContentPage_Loaded(object sender, EventArgs e)
	{
		var upgradeAvaible = await versionPopup.CheckVersion();
		if (upgradeAvaible)
			await this.ShowPopupAsync(versionPopup);
	}
}

