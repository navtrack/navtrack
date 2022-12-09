using MongoDB.Driver;

namespace Navtrack.DataAccess.Mongo;

public interface IMongoDatabaseFactory
{
    IMongoDatabase CreateMongoDatabase();
}