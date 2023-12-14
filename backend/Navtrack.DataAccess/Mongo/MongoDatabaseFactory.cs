using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Navtrack.Shared.Library.DI;

namespace Navtrack.DataAccess.Mongo;

[Service(typeof(IMongoDatabaseFactory), ServiceLifetime.Singleton)]
public class MongoDatabaseFactory : IMongoDatabaseFactory
{
    private readonly IOptions<MongoOptions> options;

    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly MongoClient mongoClient;
    private readonly IMongoDatabase mongoDatabase;

    public MongoDatabaseFactory(IOptions<MongoOptions> options)
    {
        this.options = options;
        mongoClient = new MongoClient(options.Value.ConnectionString);
        mongoDatabase = mongoClient.GetDatabase(options.Value.Database);
    }

    public IMongoDatabase CreateMongoDatabase()
    {
        ConventionRegistry.Register(nameof(IgnoreIfNullConvention),
            new ConventionPack { new IgnoreIfNullConvention(true) }, t => true);
        ConventionRegistry.Register(nameof(IgnoreExtraElementsConvention),
            new ConventionPack { new IgnoreExtraElementsConvention(true) }, t => true);

        return mongoDatabase;
    }

    public void DropDatabase()
    {
        mongoClient.DropDatabase(options.Value.Database);
    }
}