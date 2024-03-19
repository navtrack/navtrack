using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Navtrack.Shared.Library.DI;

namespace Navtrack.DataAccess.Mongo;

[Service(typeof(IMongoDatabaseProvider), ServiceLifetime.Singleton)]
public class MongoDatabaseProvider : IMongoDatabaseProvider
{
    private readonly IOptions<MongoOptions> options;
    private readonly MongoClient mongoClient;
    private readonly IMongoDatabase mongoDatabase;

    public MongoDatabaseProvider(IOptions<MongoOptions> options)
    {
        this.options = options;

        ConventionRegistry.Register(nameof(IgnoreIfDefaultConvention),
            new ConventionPack { new IgnoreIfDefaultConvention(true) }, t => true);
        ConventionRegistry.Register(nameof(IgnoreExtraElementsConvention),
            new ConventionPack { new IgnoreExtraElementsConvention(true) }, t => true);
        ConventionRegistry.Register(nameof(CamelCaseElementNameConvention),
            new ConventionPack { new CamelCaseElementNameConvention() }, t => true);

        mongoClient = new MongoClient(options.Value.ConnectionString);
        mongoDatabase = mongoClient.GetDatabase(options.Value.Database);
    }

    public IMongoDatabase GetMongoDatabase()
    {
        return mongoDatabase;
    }

    public void DropDatabase()
    {
        mongoClient.DropDatabase(options.Value.Database);
    }
}