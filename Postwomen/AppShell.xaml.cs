using Postwomen.Views;

namespace Postwomen;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute("ServiceTestPage", type: typeof(ServiceTestPage));
        Routing.RegisterRoute("MainPage", type: typeof(MainPage));
		Routing.RegisterRoute("EditCardPage", type: typeof(EditCardPage));
		Routing.RegisterRoute("SettingsPage", type: typeof(SettingsPage));
        Routing.RegisterRoute("LogsPage", type: typeof(LogsPage));
    }
}
