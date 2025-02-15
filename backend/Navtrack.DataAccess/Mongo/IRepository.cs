using MongoDB.Driver;
using System.Linq;

namespace Navtrack.DataAccess.Mongo;

public interface IRepository
{
    IQueryable<T> GetQueryable<T>() where T : class?;
    IMongoCollection<T> GetCollection<T>() where T : class;
}