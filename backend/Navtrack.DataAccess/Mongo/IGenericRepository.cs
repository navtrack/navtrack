using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Navtrack.DataAccess.Mongo;

public interface IGenericRepository<T> where T : class
{
    Task<List<T>> GetByIds(IEnumerable<ObjectId> ids);
    Task Add(T document);
    Task AddRange(IEnumerable<T> documents);
    Task<T?> GetById(string? id);
    Task<T?> GetById(ObjectId id);
    Task Delete(T document);
}