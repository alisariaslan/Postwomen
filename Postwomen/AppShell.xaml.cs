using Postwomen.Views;

namespace Postwomen;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute("mainpage", type: typeof(MainPage));
		Routing.RegisterRoute("editcard", type: typeof(EditCard));
	}
}
