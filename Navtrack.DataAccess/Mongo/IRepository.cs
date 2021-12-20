using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Navtrack.DataAccess.Mongo;

public interface IRepository
{
    IMongoQueryable<T> GetEntities<T>() where T : class;
    IMongoCollection<T> GetCollection<T>() where T : class;
}