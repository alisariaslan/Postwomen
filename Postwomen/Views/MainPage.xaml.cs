
using Postwomen.ViewModels;

namespace Postwomen.Views;

public partial class MainPage : ContentPage
{
	public MainPage(IServiceProvider services)
	{
		InitializeComponent();
		this.BindingContext = services.GetRequiredService<MainViewModel>();
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        var obj = sender as VisualElement;
        obj.IsEnabled = false;
        await obj.RelRotateTo(360, 100);
        obj.IsEnabled = true;
    }

}

