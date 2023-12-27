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
        var cards_result = sQLiteConnection.CreateTable<ServerModel>();

        if (logs_result.Equals(CreateTableResult.Created))
        {
            InsertItem(new LogsModel(LogsTypeEnum.FirstLaunch,"App launched for first time."));
        }

        if (cards_result.Equals(CreateTableResult.Created))
        {
            InsertItem(new ServerModel("Localhost", "localhost", "Example card", RemoteCallTypes.Ping));
            InsertItem(new ServerModel("Google", "google.com", "Example card", RemoteCallTypes.Ping));
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

    public async Task<int> GetItemCountAsync<T>() where T : new()
    {
        return await sQLiteAsyncConnection.Table<T>().CountAsync();
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

    public async Task<int> ClearItemsAsync<T>(T model) where T : new()
    {
        return await sQLiteAsyncConnection.DeleteAllAsync<T>();
    }
    #endregion

    #region CRUD - SYNC
    public T GetFirstItem<T>() where T : new()
    {
        return sQLiteConnection.Table<T>().FirstOrDefault();
    }

    public T GetLastItem<T>() where T : new()
    {
        return sQLiteConnection.Table<T>().LastOrDefault();
    }

    public List<T> GetItems<T>() where T : new()
    {
        return sQLiteConnection.Table<T>().ToList();
    }

    public int GetItemCount<T>() where T : new()
    {
        return sQLiteConnection.Table<T>().Count();
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

    public int DeleteFirstItem<T>(T item) where T : new()
    {
        return sQLiteConnection.Delete(GetFirstItem<T>());
    }

    public int DeleteLastItem<T>(T item) where T : new()
    {
        return sQLiteConnection.Delete(GetLastItem<T>());
    }
    #endregion

}

