namespace Postwomen.Others;

public static class Constants
{
	public const int DatabaseFileVersion = 1;

	public const string DatabaseFileExtension = ".db";

	public const string DatabaseFileName = "Postwomen";

	public const SQLite.SQLiteOpenFlags Flags =
		// open the database in read/write mode
		SQLite.SQLiteOpenFlags.ReadWrite |
		// create the database if it doesn't exist
		SQLite.SQLiteOpenFlags.Create |
		// enable multi-threaded database access
		SQLite.SQLiteOpenFlags.SharedCache;

	public static string DatabasePath =>
		Path.Combine(FileSystem.AppDataDirectory, DatabaseFileName + DatabaseFileVersion + DatabaseFileExtension);
}
