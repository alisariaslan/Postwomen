using DesenMobileDatabase.Enums;
using DesenMobileDatabase.Models;
using Postwomen.Extensions;
using Postwomen.Resources.Strings;
using Postwomen.Services;

namespace Postwomen.Views;

[QueryProperty(nameof(CardState), "CardState")]
[QueryProperty(nameof(BackAction), "BackAction")]
[QueryProperty(nameof(SelectedCard), "SelectedCard")]
public partial class EditCardPage : ContentPage
{
	public Translator Translator { get; set; } 
	private Action BackAction_ { get; set; }
    private string RBSelectionValue_ { get; set; }
    public string RBSelectionValue
    {
        get { return RBSelectionValue_; }
        set
        {
            RBSelectionValue_ = value;
            OnPropertyChanged(nameof(RBSelectionValue));
            if (Enum.TryParse<RemoteCallTypes>(value, out var parsedEnum))
                SelectedCard.TypeOfCall = parsedEnum;
        }
    }
    private int CardState_ { get; set; }
    public int CardState { get { return CardState_; } set { CardState_ = value; OnPropertyChanged(nameof(CardState)); } }
    public Command SaveCommand { get; set; }
    private ServerModel SelectedCard_ { get; set; }

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

    private IDbService dbService;

    public EditCardPage(IDbService dbService, Translator translator)
    {
        InitializeComponent();
		this.Translator = translator;
		this.dbService = dbService;
        SaveCommand = new Command(SaveCard);
        BindingContext = this;
    }

    private void InitCard()
    {
        if (CardState == 0 && SelectedCard == null)
        {
            Title = Translator["newcard"];
            SelectedCard_ = new ServerModel();
            return;
        }
        if (CardState == 1)
            Title = Translator["copycard"];
        else if (CardState == 2)
            Title = Translator["editcard"];
        OnPropertyChanged(nameof(Title));

        if (SelectedCard.IsAdvancedSettingsEnabled)
            advancedSwitch.IsToggled = true;
        RBSelectionValue = SelectedCard.TypeOfCall.ToString();
        if (SelectedCard.Port != 443 && SelectedCard.Port != 80)
            radiobutton_custom.IsChecked = true;
        OnPropertyChanged(nameof(SelectedCard));
    }

    private async void SaveCard()
    {
        try
        {
            if (string.IsNullOrEmpty(SelectedCard.Name.Trim()))
                throw new Exception(AppResources.youmustspecifyanameforyourcard);
            if (string.IsNullOrEmpty(SelectedCard.Url.Trim()))
                throw new Exception(AppResources.youmustspecifyanurloripforyourcard);
            SelectedCard.CurrentState = CheckStates.UNREACHABLE;
            bool result = false;
            if (CardState != 2)
            {
                SelectedCard.Id = 0;
                result = dbService.InsertCard(SelectedCard);
            }
            else
                 result = await dbService.UpdateCard(SelectedCard);
            if (result is false)
                throw new Exception("Insert/Update card malfunction occured!");
            await Shell.Current.Navigation.PopModalAsync();
            BackAction.Invoke();
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert(Translator["errorOccured"], ex.Message, Translator["ok"]);
        }
    }

    private void advancedSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        SelectedCard.IsAdvancedSettingsEnabled = e.Value;
    }
}

