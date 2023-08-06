
using Postwomen.ViewModels;

namespace Postwomen.Views;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
		this.BindingContext = MauiProgram.MyServiceProvider.GetRequiredService<MainViewModel>();
		(this.BindingContext as MainViewModel).MainPage = this;
	}

	private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
	{
		(sender as ListView).SelectedItem = null;
	}
}

