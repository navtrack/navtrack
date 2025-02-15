using MongoDB.Driver;
using Navtrack.Shared.Library.DI;
using System.Linq;

namespace Navtrack.DataAccess.Mongo;

[Service(typeof(IRepository))]
public class Repository(IMongoDatabaseProvider mongoDatabaseProvider) : IRepository
{
    private readonly IMongoDatabase mongoDatabase = mongoDatabaseProvider.GetMongoDatabase();

    public IQueryable<T> GetQueryable<T>() where T : class
    {
        return GetCollection<T>().AsQueryable();
    }

    public IMongoCollection<T> GetCollection<T>() where T : class
    {
        return mongoDatabase.GetCollection<T>(MongoUtils.GetCollectionName<T>());
    }
}