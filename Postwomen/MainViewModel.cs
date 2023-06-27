using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace Postwomen;

public class MainViewModel : INotifyPropertyChanged
{
	public ObservableCollection<ServerModel> ServerCards { get; set; } //NULL İKEN DATA ÇAĞIRIR
	public bool HasData { get { if (ServerCards.Count > 0) return true; else return false; } }
	public bool HasDataReversed { get { if (ServerCards.Count > 0) return false; else return true; } }
	public Command RefreshListCommand { get; set; }
	public Command CreateCardCommand { get; set; }
	public Command EditCardCommand { get; set; }
	public Command GoToWebCommand { get; set; }
	private bool IsRefreshing_ { get; set; }
	public bool IsRefreshing { get => IsRefreshing_; set { IsRefreshing_ = value; OnPropertyChanged(nameof(IsRefreshing)); } }
	private ServerModel SelectedCard_ { get; set; }
	public ServerModel SelectedCard { get { return SelectedCard_; } set { SelectedCard_ = value; } }
	public MainPage MainPage { get; set; }

	public MainViewModel()
	{
		ServerCards = new ObservableCollection<ServerModel>();
		RefreshListCommand = new Command<string>(execute: RefreshList);
		EditCardCommand = new Command<int>(execute: EditCardFunc);
		CreateCardCommand = new Command<string>(execute: CreateCardFunc);
		GoToWebCommand = new Command<string>(execute: GoToWeb);
		RefreshList("");
	}

	private async void RefreshList(string param)
	{
		IsRefreshing = true;
		await Task.Delay(3000);
		var table = await App.Database.ReadAllModels();
		foreach (var item in table)
			ServerCards.Add(item);
		IsRefreshing = false;
		OnPropertyChanged(nameof(HasData));
		OnPropertyChanged(nameof(HasDataReversed));
	}

	private async void GoToWeb(string param)
	{
		Console.WriteLine("LOG PARAMETER: " + param);
		bool yes = await MainPage.DisplayAlert("Open Browser", $"Do you wanna go to {param} ?", "Yes", "No");
		if (yes)
		{
			try
			{
				Uri uri = new Uri(param);
				await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
			}
			catch (Exception ex)
			{
				await MainPage.DisplayAlert("Error", "Maybe no internet browser avaible on this device. Ex: " + ex, "Ok");
			}
		}
	}

	private async void EditCardFunc(int param)
	{
		Console.WriteLine("LOG PARAMETER: " + param);
		var backAction = new Action(() => RefreshList(""));
		var navigationParameters = new Dictionary<string, object> { { "BackAction", backAction }, { "SelectedCard", SelectedCard } };
		await Shell.Current.GoToAsync($"editcard", navigationParameters);
		SelectedCard = null;
	}

	private async void CreateCardFunc(string param)
	{
		Console.WriteLine("LOG PARAMETER: " + param);
		var backAction = new Action(() => RefreshList(""));
		var navigationParameters = new Dictionary<string, object> { { "BackAction", backAction }, { "SelectedCard", SelectedCard } };
		await Shell.Current.GoToAsync($"editcard", navigationParameters);
		SelectedCard = null;
	}










	public event PropertyChangedEventHandler PropertyChanged;
	protected void OnPropertyChanged(string name)
	{
		Console.WriteLine($"[App] Property changed {name}");
		if (PropertyChanged != null)
			PropertyChanged(this, new PropertyChangedEventArgs(name));
	}
}
