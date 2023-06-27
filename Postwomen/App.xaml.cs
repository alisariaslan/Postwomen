namespace Postwomen;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();

	}

	private static Database database;
	public static Database Database
	{
		get
		{
			if (database == null)
			{
				database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PostwomenSQLite.db"));
			}
			return database;
		}
	}
}
