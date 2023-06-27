namespace Postwomen;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute("mainpage", typeof(MainPage));
		Routing.RegisterRoute("editcard", typeof(EditCard));
	}
}
