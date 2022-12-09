using System;
using System.Reflection;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.DataAccess.Model.Attributes;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Mongo;

[Service(typeof(IMongoRepository))]
public class MongoRepository : IMongoRepository
{
    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly IMongoClient mongoClient;
    private readonly IMongoDatabase mongoDatabase;

    public MongoRepository(IOptions<MongoOptions> mongoOptions)
    {
        mongoClient = new MongoClient(mongoOptions.Value.ConnectionString);
        mongoDatabase = mongoClient.GetDatabase(mongoOptions.Value.Database);
        
        ConventionRegistry.Register(nameof(CamelCaseElementNameConvention),
            new ConventionPack { new CamelCaseElementNameConvention() }, t => true);
        ConventionRegistry.Register(nameof(IgnoreIfNullConvention),
            new ConventionPack { new IgnoreIfNullConvention(true) }, t => true);
    }

    public IMongoQueryable<T> GetQueryable<T>()
    {
        return GetCollection<T>().AsQueryable();
    }

    public IMongoCollection<T> GetCollection<T>()
    {
        return typeof(T).GetCustomAttribute(typeof(CollectionAttribute)) is CollectionAttribute collectionAttribute
            ? mongoDatabase.GetCollection<T>(collectionAttribute.Name)
            : throw new ArgumentException($"{typeof(T).Name} does not have a Collection set.");
    }
}