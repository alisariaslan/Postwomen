using Postwomen.Models;
using SQLite;

namespace Postwomen.Others;

public class PostwomenDatabase
{

	private SQLiteAsyncConnection sQLiteAsyncConnection;

	public PostwomenDatabase()
	{
	}

	async Task Init()
	{
		if (sQLiteAsyncConnection is not null)
			return;

		sQLiteAsyncConnection = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
#if DEBUG
		//await DropTableAsync< ServerModel>();
#endif
		await sQLiteAsyncConnection.CreateTableAsync<ServerModel>();
	}

	public async Task<int> DropTableAsync<T>()
	{
		await Init();
		if (typeof(T) == typeof(ServerModel))
		{
			var result = await sQLiteAsyncConnection.DropTableAsync<ServerModel>();
			await Init();
			return result;
		}
		else
			return 0;
	}

	public async Task<List<ServerModel>> GetItemsAsync()
	{
		await Init();
		return await sQLiteAsyncConnection.Table<ServerModel>().ToListAsync();
	}

	public async Task<ServerModel> GetItemAsync(int id)
	{
		await Init();
		return await sQLiteAsyncConnection.FindAsync<ServerModel>(id);
	}

	public async Task<int> InsertItemAsync(ServerModel item)
	{
		await Init();
		return await sQLiteAsyncConnection.InsertAsync(item);
	}

	public async Task<int> UpdateItemAsync(ServerModel item)
	{
		await Init();
		return await sQLiteAsyncConnection.UpdateAsync(item);
	}

	public async Task<int> DeleteItemAsync(ServerModel item)
	{
		await Init();
		return await sQLiteAsyncConnection.DeleteAsync(item);
	}

	public async Task<int> SaveItemAsync(ServerModel item)
	{
		await Init();
		var result = await sQLiteAsyncConnection.FindAsync<ServerModel>(item.Id);
		if (result == null)
			return await InsertItemAsync(item);
		else
			return await UpdateItemAsync(item);
	}

}
