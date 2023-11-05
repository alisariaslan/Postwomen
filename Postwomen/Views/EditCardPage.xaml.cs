using Postwomen.Models;
using Postwomen.Others;
using Postwomen.Resources.Strings;

namespace Postwomen.Views;

[QueryProperty(nameof(CardState), "CardState")]
[QueryProperty(nameof(BackAction), "BackAction")]
[QueryProperty(nameof(SelectedCard), "SelectedCard")]
public partial class EditCardPage : ContentPage
{
    private Action BackAction_ { get; set; }
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
    private int CardState_ { get; set; }
    public int CardState { get { return CardState_; } set { CardState_ = value; OnPropertyChanged(nameof(CardState)); } }
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

    public EditCardPage(PostwomenDatabase postwomenDatabase)
    {
        InitializeComponent();
        MyPostwomenDatabase = postwomenDatabase;
        BindingContext = this;
        SaveCommand = new Command(SaveCard);
        OnPropertyChanged(nameof(SaveCommand));
    }

    private void InitCard()
    {
        if (CardState == 0 && SelectedCard == null)
        {
            Title = AppResources.newcard;
            SelectedCard_ = new ServerModel();
            return;
        }
        if (CardState == 1)
            Title = AppResources.copycard;
        else if (CardState == 2)
            Title = AppResources.editcard;
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
            if (CardState == 1)
                SelectedCard.Id = 0;
            if (string.IsNullOrEmpty(SelectedCard.Name.Trim()))
                throw new Exception(AppResources.youmustspecifyanameforyourcard);
                       if (string.IsNullOrEmpty(SelectedCard.Url.Trim()))
                throw new Exception(AppResources.youmustspecifyanurloripforyourcard);
            await MyPostwomenDatabase.SaveServerCardAsync(SelectedCard);
            await Shell.Current.Navigation.PopModalAsync();
            BackAction.Invoke();
        }
        catch (Exception e)
        {
            await App.Current.MainPage.DisplayAlert(AppResources.error, e.Message, AppResources.ok);
        }
        finally
        {

        }

    }

    private void advancedSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        SelectedCard.IsAdvancedSettingsEnabled = e.Value;
    }
}

