using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Mongo;

[Service(typeof(IMongoDatabaseFactory))]
// ReSharper disable once InconsistentNaming
public class MongoDatabaseFactory : IMongoDatabaseFactory
{
    private readonly IOptions<MongoOptions> mongoOptions;

    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private IMongoClient mongoClient;
    private IMongoDatabase mongoDatabase;

    public MongoDatabaseFactory(IOptions<MongoOptions> mongoOptions)
    {
        this.mongoOptions = mongoOptions;
    }

    public IMongoDatabase CreateMongoDatabase()
    {
        mongoClient = new MongoClient(mongoOptions.Value.ConnectionString);
        mongoDatabase = mongoClient.GetDatabase(mongoOptions.Value.Database);
            
        ConventionRegistry.Register(nameof(IgnoreIfNullConvention),
            new ConventionPack { new IgnoreIfNullConvention(true) }, t => true);
        ConventionRegistry.Register(nameof(IgnoreExtraElementsConvention),
            new ConventionPack { new IgnoreExtraElementsConvention(true) }, t => true);

        return mongoDatabase;
    }
}