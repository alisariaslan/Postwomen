using Postwomen.Enums;
using Postwomen.Models;
using Postwomen.Others;
using System.Collections.ObjectModel;

namespace Postwomen.ViewModels;

public class MainViewModel : BaseViewModel
{
	private PostwomenDatabase MyPostwomenDatabase { get; set; }
	public ObservableCollection<ServerModel> ServerCards { get; set; } //NULL İKEN DATA ÇAĞIRIR
	public bool HasData { get { if (ServerCards.Count > 0) return true; else return false; } }
	public bool HasDataReversed { get { if (ServerCards.Count > 0) return false; else return true; } }
	public Command RefreshListCommand { get; set; }
	public Command ResetDBCommand { get; set; }
	public Command CreateNewCardCommand { get; set; }
	public Command EditCardCommand { get; set; }
	public Command GoToWebCommand { get; set; }
	public Command CopyCardCommand { get; set; }
	public Command DeleteCardCommand { get; set; }
	public Command SettingsCommand { get; set; }
	private bool IsRefreshing_ { get; set; }
	public bool IsRefreshing { get => IsRefreshing_; set { IsRefreshing_ = value; OnPropertyChanged(nameof(IsRefreshing)); } }
	private ServerModel SelectedCard_ { get; set; }
	public ServerModel SelectedCard { get { return SelectedCard_; } set { SelectedCard_ = value; } }

	public MainViewModel(PostwomenDatabase postwomenDatabase)
	{
		MyPostwomenDatabase = postwomenDatabase;
		ServerCards = new ObservableCollection<ServerModel>();
		RefreshListCommand = new Command<string>(execute: RefreshList);
		EditCardCommand = new Command<int>(execute: EditCardFunc);
		CopyCardCommand = new Command<int>(execute: CopyCardFunc);
		DeleteCardCommand = new Command<int>(execute: DeleteCardFunc);
		CreateNewCardCommand = new Command<string>(execute: CreateNewCardFunc);
		GoToWebCommand = new Command<string>(execute: GoToWebFunc);
		ResetDBCommand = new Command<string>(execute: ResetDB);
		SettingsCommand = new Command(GoToSettingsFunc);
		RefreshList("");
	}

	private async void ResetDB(string param)
	{
		bool answer = await App.Current.MainPage.DisplayAlert("Reset database", "Are you really want to reset your database?", "Yes", "No");
		if (answer is false)
			return;

		try
		{
			var result = await MyPostwomenDatabase.DropTableAsync<ServerModel>();
			await App.Current.MainPage.DisplayAlert("Operation Result", ((BasicOperations)result).ToString(), "Ok");
			if (result == (int)BasicOperations.SUCCESS)
			{
				await App.Current.MainPage.DisplayAlert("Warning!", "Please restart the application!", "Ok");
				Application.Current.Quit();
			}
		}
		catch (Exception e)
		{
			await App.Current.MainPage.DisplayAlert("Error occured", e.Message, "Ok");
		}

	}

	private async void RefreshList(string param)
	{
		await Task.Delay(100);
		var items = await MyPostwomenDatabase.GetItemsAsync();
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
		var items = await MyPostwomenDatabase.GetItemsAsync();
		ServerCards.Clear();
		foreach (var item in items)
			ServerCards.Add(item);
		OnPropertyChanged(nameof(ServerCards));
	}

	private async void GoToSettingsFunc()
	{
		await Shell.Current.GoToAsync($"settings");
	}

	private async void GoToWebFunc(string param)
	{
		Console.WriteLine("LOG PARAMETER: " + param);
		bool yes = await App.Current.MainPage.DisplayAlert("Open Browser", $"Do you wanna go to {param} ?", "Yes", "No");
		if (yes)
		{
			try
			{
				Uri uri = new Uri(param);
				await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
			}
			catch (Exception ex)
			{
				await App.Current.MainPage.DisplayAlert("Error", "Maybe no internet browser avaible on this device. Ex: " + ex, "Ok");
			}
		}
	}

	private async void CreateNewCardFunc(string param)
	{
		Console.WriteLine("LOG PARAMETER: " + param);
		var backAction = new Action(() => RefreshList(""));
		var navigationParameters = new Dictionary<string, object> { { "BackAction", backAction }, { "SelectedCard", SelectedCard } };
		await Shell.Current.GoToAsync($"editcard", navigationParameters);
		SelectedCard = null;
	}

	private async void CopyCardFunc(int param)
	{
		SelectedCard = await MyPostwomenDatabase.GetItemAsync(param);
		Console.WriteLine("LOG PARAMETER: " + param);
		var backAction = new Action(() => RefreshList(""));
		var navigationParameters = new Dictionary<string, object> {
			{"BackAction", backAction},
			{"SelectedCard", SelectedCard},
			{"IsCopy",true}
		};
		await Shell.Current.GoToAsync($"editcard", navigationParameters);
		SelectedCard = null;
	}

	private async void EditCardFunc(int param)
	{
		SelectedCard = await MyPostwomenDatabase.GetItemAsync(param);
		Console.WriteLine("LOG PARAMETER: " + param);
		var backAction = new Action(() => RefreshList(""));
		var navigationParameters = new Dictionary<string, object> {
			{"BackAction", backAction},
			{"SelectedCard", SelectedCard}
		};
		await Shell.Current.GoToAsync($"editcard", navigationParameters);
		SelectedCard = null;
	}

	private async void DeleteCardFunc(int param)
	{
		bool result = await App.Current.MainPage.DisplayAlert("Deleting card", "Do you really want to delete this item?", "Yes", "No");
		if (result is false)
			return;
		SelectedCard = await MyPostwomenDatabase.GetItemAsync(param);
		await MyPostwomenDatabase.DeleteItemAsync(SelectedCard);
		RefreshList("");
		SelectedCard = null;
	}




}
