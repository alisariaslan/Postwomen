using DesenMobileDatabase;
using DesenMobileDatabase.Models;

namespace Postwomen.Services;

public interface IDbService
{
    public bool InsertLog(LogsModel model);

    public Task<int> ClearLogs();

    public Task<List<LogsModel>> GetLogs();

    public int GetLogCount();

    public bool InsertCard(ServerModel model);

    public Task<int> ClearCards();

    public Task<List<ServerModel>> GetCards();

    public Task<bool> UpdateCard(ServerModel model);

    public Task<bool> DelCard(ServerModel model);


}

public class DbService : IDbService
{
    private MobileDB db;
    public DbService(MobileDB db)
    {
        this.db = db;
    }

    public bool InsertLog(LogsModel model)
    {
        while (GetLogCount() >= Preferences.Get("MaxLogCount", 500))
            db.DeleteFirstItem(model);
        return db.InsertItem(model) > 0 ? true : false;
    }

    public async Task<int> ClearLogs()
    {
        return await db.ClearItemsAsync(new LogsModel());
    }

    public async Task<List<LogsModel>> GetLogs()
    {
        return await db.GetItemsAsync<LogsModel>();
    }

    public int GetLogCount()
    {
        return db.GetItemCount<LogsModel>();
    }

    public bool InsertCard(ServerModel model)
    {
        return db.InsertItem(model) > 0 ? true : false;
    }

    public async Task<int> ClearCards()
    {
        return await db.ClearItemsAsync(new ServerModel());
    }

    public async Task<List<ServerModel>> GetCards()
    {
        return await db.GetItemsAsync<ServerModel>();
    }

    public async Task<bool> UpdateCard(ServerModel model)
    {
        return await db.UpdateItemAsync(model) > 0 ? true : false;
    }
    public async Task<bool> DelCard(ServerModel model)
    {
        return await db.DeleteItemAsync(model) > 0 ? true : false;
    }
}
