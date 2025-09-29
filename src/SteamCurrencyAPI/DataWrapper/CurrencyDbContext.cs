using MongoDB.Bson;
using MongoDB.Driver;
using SteamCurrencyAPI.Interfaces;
using SteamCurrencyAPI.Models;

namespace SteamCurrencyAPI.DataWrapper;

public class CurrencyDbContext : ICurrencyDbContext
{
    private readonly IMongoCollection<Currency> _currencyCollection;
    //yes, chatgpt help me with this class(
    public CurrencyDbContext(IConfiguration configuration)
    {
        var mongoDbSettings = configuration.GetSection("MongoDbSettings");

        var connectionString = mongoDbSettings["ConnectionString"] ?? throw new Exception("connectionString notFound");
        var databaseName = mongoDbSettings["DatabaseName"] ?? throw new Exception("databaseName notFound");
        var collectionName = mongoDbSettings["CollectionName"] ?? throw new Exception("collectionName notFound");

        Console.WriteLine(connectionString);

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _currencyCollection = database.GetCollection<Currency>(collectionName);
    }

    public async Task<CurrencyData> GetLatestCurrencyByCodeAsync(string currencyCode)
    {
        var pipeline = new BsonDocument[]
        {
            new("$match", new BsonDocument("CurrencyCode", currencyCode)),
            new("$unwind", "$CurrencyDatas"),
            new("$sort", new BsonDocument("CurrencyDatas.DateAtUtc", -1)),
            new("$limit", 1),
            new("$project", new BsonDocument
            {
                { "DateAtUtc", "$CurrencyDatas.DateAtUtc" },
                { "CurrencyPrice", "$CurrencyDatas.CurrencyPrice" }
            })
        };

        var cursor = await _currencyCollection.AggregateAsync<CurrencyData>(pipeline);
        return await cursor.FirstOrDefaultAsync();
    }




    public async Task<List<CurrencyInfo>> GetCurrencyInRangeAsync(string currencyCode, DateOnly start, DateOnly end)
    {
        var filter = Builders<Currency>.Filter.Eq(c => c.CurrencyCode, currencyCode);
        var currency = await _currencyCollection.Find(filter).FirstOrDefaultAsync();

        var startDate = start.ToDateTime(TimeOnly.MinValue).Date;
        var endDate = end.ToDateTime(TimeOnly.MinValue).Date;

        var results = currency?.CurrencyDatas
            .Where(d => d.DateAtUtc.Date >= startDate && d.DateAtUtc.Date <= endDate)
            .Select(d => new CurrencyInfo
            {
                DateAtUtc = DateOnly.FromDateTime(d.DateAtUtc),
                CurrencyPrice = d.CurrencyPrice
            })
            .ToList();

        return results ?? new List<CurrencyInfo>();
    }


    public async Task AddCurrencyDateAsync(string currencyCode, CurrencyData newCurrencyDate)
    {
        var filter = Builders<Currency>.Filter.Eq(c => c.CurrencyCode, currencyCode);
        var update = Builders<Currency>.Update
            .Push(c => c.CurrencyDatas, newCurrencyDate)
            .SetOnInsert(c => c.CreatedAtUTC, DateTime.UtcNow);

        await _currencyCollection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
    }

    public async Task<List<string>> GetAllCurrencyCodesAsync()
    {
        var filter = Builders<Currency>.Filter.Empty;
        using var cursor = await _currencyCollection.DistinctAsync(c => c.CurrencyCode, filter);
        return await cursor.ToListAsync();
    }

    public async Task<List<Currency>> GetAllCurrenciesAsync()
    {
        return await _currencyCollection.Find(_ => true)
            .SortByDescending(c => c.CreatedAtUTC)
            .ToListAsync();
    }
}