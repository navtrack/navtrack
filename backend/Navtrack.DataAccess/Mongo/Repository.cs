using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Mongo;

[Service(typeof(IRepository))]
public class Repository : IRepository
{
    private readonly IMongoDatabase mongoDatabase;

    private readonly IMongoDatabaseFactory mongoDatabaseFactory;
    // private readonly IInterceptorService interceptorService;

    public Repository(IMongoDatabaseFactory mongoDatabaseFactory)
    {
        this.mongoDatabaseFactory = mongoDatabaseFactory;
        mongoDatabase = mongoDatabaseFactory.CreateMongoDatabase();
    }

    public IMongoQueryable<T> GetEntities<T>() where T : class
    {
        return GetCollection<T>().AsQueryable();
    }

    public IMongoCollection<T> GetCollection<T>() where T : class
    {
        return mongoDatabase.GetCollection<T>(MongoUtils.GetCollectionName<T>());
    }
}