namespace Postwomen;

[QueryProperty(nameof(BackAction), "BackAction")]
[QueryProperty(nameof(SelectedCard), "SelectedCard")]
public partial class EditCard : ContentPage
{
	private Action BackAction_ { get; set; }
	public Action BackAction
	{
		get => BackAction_;
		set
		{
			BackAction_ = value;
			OnPropertyChanged(nameof(BackAction));
		}
	}
	private ServerModel SelectedCard_ { get; set; }
	public ServerModel SelectedCard
	{
		get => SelectedCard_;
		set
		{
			SelectedCard_ = value; OnPropertyChanged();
			if (value == null)
				IsNewCard = true;
			InitCard();
		}
	}
	public bool IsNewCard { get; set; }
	public Command SaveCommand { get; set; }

	public EditCard()
	{
		InitializeComponent();
		BindingContext = this;
		SaveCommand = new Command(SaveCard);
		OnPropertyChanged(nameof(SaveCommand));
	}

	private void InitCard()
	{
		if (IsNewCard)
			Title = "New Card";
	}

	private async void SaveCard()
	{
		Console.WriteLine("Shell Navigate!");
		await Shell.Current.Navigation.PopModalAsync();
		BackAction.Invoke();
	}

}

