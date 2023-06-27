using SQLite;

namespace Postwomen;


public class Database
{
	readonly SQLiteAsyncConnection database;

	public Database(string dbPath)
	{
		database = new SQLiteAsyncConnection(dbPath);
		database.CreateTableAsync<ServerModel>().Wait();
	}

	public async Task<int> CreateModel(ServerModel model)
	{
		LogThis(model);
		return LogThis(await database.InsertAsync(model));
	}

	public async Task<List<ServerModel>> ReadAllModels()
	{
		//var result = from s in await database.Table<ServerModel>().ToListAsync() orderby s.Id select s;
		//return result;
		return LogThis(await database.Table<ServerModel>().ToListAsync());
	}

	public async Task<int> UpdateModel(ServerModel model)
	{
		LogThis(model);
		return LogThis(await database.UpdateAsync(model));
	}

	public async Task<int> DeleteModel(ServerModel model)
	{
		LogThis(model);
		return LogThis(await database.DeleteAsync(model));
	}

	public async Task<int> ClearAllModels()
	{
		return LogThis(await database.DeleteAllAsync<ServerModel>());
	}

	public void LogThis(ServerModel model)
	{
		Console.WriteLine("RECORD DETAILS");
		Console.WriteLine("Id: "+model.Id);
		Console.WriteLine("Name: " + model.Name);
		Console.WriteLine("Description: " + model.Description);
		Console.WriteLine("Url: " + model.Url);
		Console.WriteLine("Port: " + model.Port);
		Console.WriteLine("State: " + model.State);
		Console.WriteLine("AutoCheckEnabled: " + model.AutoCheckEnabled);
		Console.WriteLine("NotificationEnabled: " + model.NotificationEnabled);
	}

	public int LogThis(int i)
	{
		Console.WriteLine("Affected Records: " + i);
		return i;
	}
	public List<ServerModel> LogThis(List<ServerModel> models)
	{
		Console.WriteLine("Count of Records: " + models.Count);
		return models;
	}


	//public Task<ServerModel> GetLoginDataAsync(string userName)
	//{
	//	return database.Table<ServerModel>()
	//					.Where(i => i.UserName == userName)
	//					.FirstOrDefaultAsync();
	//}

	//public async void ClearLoginDataAsync()
	//{
	//	  await database.Table<ServerModel>().Where(x => true).DeleteAsync();
	//}

	//public Task<List<LoginMode>> GetLoginListAsync()
	//{
	//	return database.Table<LoginMode>().OrderByDescending(i => i.Date)
	//					.ToListAsync();
	//}

	//public async Task<List<ServerModel>> GetLoginListAsync()
	//{
	//	// Order records by date in descending order
	//	var records = await database.Table<ServerModel>()

	//								.ToListAsync();

	//	// Check if the number of records is greater than 10
	//	if (records.Count > 10)
	//	{
	//		// Calculate the number of records to delete
	//		int recordsToDelete = records.Count - 10;

	//		// Select the records to delete
	//		var recordsToDeleteIds = records.Take(recordsToDelete).Select(r => r.Date);

	//		// Delete the records
	//		await database.Table<ServerModel>()
	//					  .DeleteAsync(r => recordsToDeleteIds.Contains(r.Date));

	//		// Remove the deleted records from the list
	//		records.RemoveAll(r => recordsToDeleteIds.Contains(r.Date));
	//	}

	//	return records.OrderByDescending(r => r.Date).ToList();
	//}

	//public Task<int> SaveLoginDataAsync(ServerModel loginData)
	//{
	//	return database.InsertAsync(loginData);
	//}


	/*
	 	var records = await database.Table<ServerModel>().ToListAsync();
		int recordsToDelete = records.Count;
		// Select the records to delete
		var recordsToDeleteIds = records.Take(recordsToDelete).Select(r => r.Id);
		// Delete the records
		await database.Table<ServerModel>()
					  .DeleteAsync(r => recordsToDeleteIds.Contains(r.Id));
		// Remove the deleted records from the list
		return records.RemoveAll(r => recordsToDeleteIds.Contains(r.Id));
	*/
}
