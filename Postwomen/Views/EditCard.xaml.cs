using Postwomen.Enums;
using Postwomen.Models;
using Postwomen.Others;

namespace Postwomen.Views;

[QueryProperty(nameof(IsCopy), "IsCopy")]
[QueryProperty(nameof(BackAction), "BackAction")]
[QueryProperty(nameof(SelectedCard), "SelectedCard")]
public partial class EditCard : ContentPage
{
	private Action BackAction_ { get; set; }

	private bool IsNewCard_ { get; set; }
	public bool IsNewCard { get { return IsNewCard_; } set { IsNewCard_ = value; OnPropertyChanged(nameof(IsNewCard)); } }

	private string RBSelectionValue_ { get; set; }
	public string RBSelectionValue
	{
		get { return RBSelectionValue_; }
		set
		{
			RBSelectionValue_ = value;
			OnPropertyChanged(nameof(RBSelectionValue));
			SelectedCard.TypeOfCall = Convert.ToInt32(value);
		}
	}

	public bool IsCopy { get; set; }
	public Command SaveCommand { get; set; }
	private ServerModel SelectedCard_ { get; set; }
	private PostwomenDatabase MyPostwomenDatabase { get; set; }

	public Action BackAction
	{
		get => BackAction_;
		set
		{
			BackAction_ = value;
			OnPropertyChanged(nameof(BackAction));
		}
	}

	public ServerModel SelectedCard
	{
		get => SelectedCard_;
		set
		{
			SelectedCard_ = value;
			OnPropertyChanged(nameof(SelectedCard));
			InitCard();
		}
	}

	public EditCard(PostwomenDatabase postwomenDatabase)
	{
		InitializeComponent();
		MyPostwomenDatabase = postwomenDatabase;
		BindingContext = this;
		SaveCommand = new Command(SaveCard);
		OnPropertyChanged(nameof(SaveCommand));
	}

	private void InitCard()
	{
		if (SelectedCard == null)
		{
			IsNewCard = true;
			Title = "New Card";
			SelectedCard = new ServerModel();
		}
		else if (IsCopy)
		{
			Title = "Copy Card";
		}
		else
		{
			Title = "Edit Card";
		}

		if (SelectedCard.Port == 443)
			radiobutton_https.IsChecked = true;
		else if (SelectedCard.Port == 80)
			radiobutton_http.IsChecked = true;
		else
			radiobutton_custom.IsChecked = true;

		if (SelectedCard.IsAdvancedSettingsEnabled)
			advancedSwitch.IsToggled = true;

		RBSelectionValue = SelectedCard.TypeOfCall.ToString();
	}

	private async void SaveCard()
	{
		try
		{
			if (IsCopy)
				SelectedCard.Id = 0;
			await MyPostwomenDatabase.SaveItemAsync(SelectedCard);
			await Shell.Current.Navigation.PopModalAsync();
			BackAction.Invoke();
		}
		catch (Exception e)
		{
			await App.Current.MainPage.DisplayAlert("Error occured", e.Message, "Ok");
		}
		finally
		{

		}

	}

	private void advancedSwitch_Toggled(object sender, ToggledEventArgs e)
	{
		SelectedCard.IsAdvancedSettingsEnabled = advancedSwitch.IsToggled;
	}
}

