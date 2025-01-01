using EventSourcingApplication.Shared.Services.Abstractions;
using MongoDB.Driver;

namespace EventSourcingApplication.Shared.Services;

public class MongoDbService : IMongoDbService
{
    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        IMongoDatabase mongoDatabase = GetDatabase();
        return mongoDatabase.GetCollection<T>(collectionName);
    }

    public IMongoDatabase GetDatabase(string databaseName = "ProductDb", string connectionString = "mongodb://localhost:27017")
    {
        MongoClient mongoClient = new(connectionString);
        
        return mongoClient.GetDatabase(databaseName);
    }
}