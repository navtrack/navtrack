using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Navtrack.DataAccess.Mongo;

public interface IMongoRepository
{
    IMongoQueryable<T> GetQueryable<T>();
    IMongoCollection<T> GetCollection<T>();
}