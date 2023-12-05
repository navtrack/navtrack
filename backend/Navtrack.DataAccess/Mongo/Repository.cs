using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.Shared.Library.DI;

namespace Navtrack.DataAccess.Mongo;

[Service(typeof(IRepository))]
public class Repository(IMongoDatabaseFactory mongoDatabaseFactory) : IRepository
{
    private readonly IMongoDatabase mongoDatabase = mongoDatabaseFactory.CreateMongoDatabase();

    private readonly IMongoDatabaseFactory mongoDatabaseFactory = mongoDatabaseFactory;
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