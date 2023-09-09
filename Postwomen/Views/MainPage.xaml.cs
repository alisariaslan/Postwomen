
using Postwomen.ViewModels;

namespace Postwomen.Views;

public partial class MainPage : ContentPage
{

	public MainPage(IServiceProvider services)
	{
		InitializeComponent();
		this.BindingContext = services.GetRequiredService<MainViewModel>();
	}
}

