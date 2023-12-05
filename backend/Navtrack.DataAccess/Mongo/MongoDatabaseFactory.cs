using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Navtrack.Shared.Library.DI;

namespace Navtrack.DataAccess.Mongo;

[Service(typeof(IMongoDatabaseFactory))]
public class MongoDatabaseFactory(IOptions<MongoOptions> options) : IMongoDatabaseFactory
{
    private IMongoClient mongoClient;
    private IMongoDatabase mongoDatabase;

    public IMongoDatabase CreateMongoDatabase()
    {
        mongoClient = new MongoClient(options.Value.ConnectionString);
        mongoDatabase = mongoClient.GetDatabase(options.Value.Database);
            
        ConventionRegistry.Register(nameof(IgnoreIfNullConvention),
            new ConventionPack { new IgnoreIfNullConvention(true) }, t => true);
        ConventionRegistry.Register(nameof(IgnoreExtraElementsConvention),
            new ConventionPack { new IgnoreExtraElementsConvention(true) }, t => true);

        return mongoDatabase;
    }
}