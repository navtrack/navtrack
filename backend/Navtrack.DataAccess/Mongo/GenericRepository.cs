using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Navtrack.DataAccess.Mongo;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseDocument
{
    protected readonly IRepository repository;

    protected GenericRepository(IRepository repository)
    {
        this.repository = repository;
    }

    public virtual Task<List<T>> GetByIds(IEnumerable<ObjectId> ids)
    {
        return repository.GetCollection<T>().Find(Builders<T>.Filter.In(x => x.Id, ids)).ToListAsync();
    }

    public virtual Task Add(T document)
    {
        return repository.GetCollection<T>().InsertOneAsync(document);
    }

    public Task AddRange(IEnumerable<T> documents)
    {
        return repository.GetCollection<T>().InsertManyAsync(documents);
    }

    public Task<T?> GetById(string? id)
    {
        if (ObjectId.TryParse(id, out ObjectId objectId))
        {
            return GetById(objectId);
        }

        return Task.FromResult(default(T));
    }

    public Task<T?> GetById(ObjectId id)
    {
        return repository.GetQueryable<T?>().FirstOrDefaultAsync(x => x!.Id == id);
    }

    public virtual Task Delete(T document)
    {
        return repository.GetCollection<T>().DeleteOneAsync(x => x.Id == document.Id);
    }
}