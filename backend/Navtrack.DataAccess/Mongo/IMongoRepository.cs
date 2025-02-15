
using MongoDB.Driver;
using System.Linq;
namespace Navtrack.DataAccess.Mongo;

public interface IMongoRepository
{
    IQueryable<T> GetQueryable<T>();
    IMongoCollection<T> GetCollection<T>();
}