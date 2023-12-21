using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.Shared.Library.DI;

namespace Navtrack.DataAccess.Mongo;

[Service(typeof(IRepository))]
public class Repository(IMongoDatabaseProvider mongoDatabaseProvider) : IRepository
{
    private readonly IMongoDatabase mongoDatabase = mongoDatabaseProvider.GetMongoDatabase();

    private readonly IMongoDatabaseProvider mongoDatabaseProvider = mongoDatabaseProvider;
    // private readonly IInterceptorService interceptorService;

    public IMongoQueryable<T> GetQueryable<T>() where T : class
    {
        return GetCollection<T>().AsQueryable();
    }

    public IMongoCollection<T> GetCollection<T>() where T : class
    {
        return mongoDatabase.GetCollection<T>(MongoUtils.GetCollectionName<T>());
    }
}