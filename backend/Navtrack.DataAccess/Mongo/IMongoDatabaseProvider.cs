using MongoDB.Driver;

namespace Navtrack.DataAccess.Mongo;

public interface IMongoDatabaseProvider
{
    IMongoDatabase GetMongoDatabase();
    void DropDatabase();
}