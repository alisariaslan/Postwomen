using Postwomen.Models;
using SQLite;

namespace Postwomen.Others;

public class PostwomenDatabase
{
    private SQLiteAsyncConnection sQLiteAsyncConnection;

    async Task Init()
    {
        if (sQLiteAsyncConnection is not null)
            return;

        sQLiteAsyncConnection = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

        await sQLiteAsyncConnection.CreateTableAsync<ServerModel>();
        await sQLiteAsyncConnection.CreateTableAsync<LogModel>();
    }

    public async Task<int> DropTableAsync<T>() where T : new()
    {
        await Init();
        var result = await sQLiteAsyncConnection.DropTableAsync<T>();
        return result;
    }

    public async Task<List<T>> GetItemsAsync<T>() where T : new()
    {
        await Init();
        return await sQLiteAsyncConnection.Table<T>().ToListAsync();
    }

    public async Task<int> InsertItemAsync<T>(T item) where T : new()
    {
        await Init();
        return await sQLiteAsyncConnection.InsertAsync(item);
    }

    public async Task<int> UpdateItemAsync<T>(T item) where T : new()
    {
        await Init();
        return await sQLiteAsyncConnection.UpdateAsync(item);
    }

    public async Task<int> DeleteItemAsync<T>(T item) where T : new()
    {
        await Init();
        return await sQLiteAsyncConnection.DeleteAsync(item);
    }

    public async Task<ServerModel> GetServerCard(int id)
    {
        await Init();
        return await sQLiteAsyncConnection.FindAsync<ServerModel>(id);
    }

    public async Task<int> SaveServerCardAsync(ServerModel item)
    {
        await Init();
        var result = await sQLiteAsyncConnection.FindAsync<ServerModel>(item.Id);
        if (result == null)
            return await InsertItemAsync(item);
        else
            return await UpdateItemAsync(item);
    }

    public async Task<int> SaveLogAsync(string desc)
    {
        var mlc = Convert.ToInt32(Preferences.Get("MaxLogCount", 5000));
        var lc = Convert.ToInt32(Preferences.Get("LogCount", 5001));

        if (lc > mlc)
        {
            int deletedCount = 0;
            var logs = await GetItemsAsync<LogModel>();
            lc = logs.Count;
            foreach (var log in logs)
            {
                int result = await sQLiteAsyncConnection.DeleteAsync(log);
                if (result > 0)
                    deletedCount++;
                if (logs.Count - deletedCount < mlc)
                    break;
            }
            lc -= deletedCount;
        }
        lc += 1;
        Preferences.Set("LogCount", lc);

        await Init();
        LogModel item = new LogModel() { Description = desc };
        return await sQLiteAsyncConnection.InsertAsync(item);
    }
}
