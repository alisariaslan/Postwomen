using Postwomen.ViewModels;

namespace Postwomen.Views;

public partial class SettingsView : ContentPage
{
	
	public SettingsView()
	{
		InitializeComponent();
		this.BindingContext = new SettingsViewModel();
	}


}

