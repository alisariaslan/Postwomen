using System.Globalization;
using DesenMobileDatabase.Enums;
using DesenMobileDatabase.Models;
using SQLite;

namespace DesenMobileDatabase;

public class MobileDB
{
    private SQLiteConnection sQLiteConnection { get; set; }
    private SQLiteAsyncConnection sQLiteAsyncConnection { get; set; }

    public MobileDB()
    {

    }

    public void Init()
    {
        if (sQLiteConnection is not null)
            return;

        sQLiteConnection = new SQLiteConnection(DBSettings.GetPath, DBSettings.GetFlags);

        var settings_result = sQLiteConnection.CreateTable<SettingsModel>();
        var logs_result = sQLiteConnection.CreateTable<LogsModel>();
        var users_result = sQLiteConnection.CreateTable<SavedUserModel>();

        if (settings_result.Equals(CreateTableResult.Created))
        {
            InsertItem(new SettingsModel(SettingsTypeEnum.Language, CultureInfo.CurrentCulture.TwoLetterISOLanguageName));
            InsertItem(new SettingsModel(SettingsTypeEnum.Theme, Application.Current.UserAppTheme.ToString()));
        }
    }

    public async Task InitAsync()
    {
        if (sQLiteAsyncConnection is not null)
            return;

        sQLiteAsyncConnection = new SQLiteAsyncConnection(DBSettings.GetPath, DBSettings.GetFlags);
    }

    #region CRUD - ASYNC
    public async Task<List<T>> GetItemsAsync<T>() where T : new()
    {
        return await sQLiteAsyncConnection.Table<T>().ToListAsync();
    }

    public async Task<int> InsertItemAsync<T>(T item) where T : new()
    {
        return await sQLiteAsyncConnection.InsertAsync(item);
    }

    public async Task<int> UpdateItemAsync<T>(T item) where T : new()
    {
        return await sQLiteAsyncConnection.UpdateAsync(item);
    }

    public async Task<int> DeleteItemAsync<T>(T item) where T : new()
    {
        return await sQLiteAsyncConnection.DeleteAsync(item);
    }
    #endregion

    #region CRUD - SYNC
    public List<T> GetItems<T>() where T : new()
    {
        return sQLiteConnection.Table<T>().ToList();
    }

    public int InsertItem<T>(T item) where T : new()
    {
        return sQLiteConnection.Insert(item);
    }

    public int UpdateItem<T>(T item) where T : new()
    {
        return sQLiteConnection.Update(item);
    }

    public int DeleteItem<T>(T item) where T : new()
    {
        return sQLiteConnection.Delete(item);
    }
    #endregion

}

