namespace DesenMobileDatabase;

public class DBSettings
{
    private const int DB_Version = 4;

    private const string DB_Extension = ".yemekpos";

    private const string DB_Name = "storage";

    private const SQLite.SQLiteOpenFlags DB_Flags =
        // open the database in read/write mode
        SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLite.SQLiteOpenFlags.SharedCache;

    public static string GetPath => Path.Combine(FileSystem.AppDataDirectory, DB_Name  + DB_Version + DB_Extension);

    public static SQLite.SQLiteOpenFlags GetFlags => DB_Flags;

    public DBSettings()
	{
		
	}
}

