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

    public Task<List<T>> GetByIds(IEnumerable<ObjectId> ids)
    {
        return GetCollection().Find(Builders<T>.Filter.In(x => x.Id, ids)).ToListAsync();
    }

    public IMongoQueryable<T> GetQueryable()
    {
        return repository.GetQueryable<T>();
    }

    public IMongoCollection<T> GetCollection()
    {
        return repository.GetCollection<T>();
    }

    public Task Add(T document)
    {
        return repository.GetCollection<T>().InsertOneAsync(document);
    }

    public Task AddRange(IEnumerable<T> documents)
    {
        return repository.GetCollection<T>().InsertManyAsync(documents);
    }

    public Task<T> GetById(string id)
    {
        return repository.GetQueryable<T>().FirstOrDefaultAsync(x => x.Id == ObjectId.Parse(id));
    }

    public Task<T> GetById(ObjectId id)
    {
        
        return repository.GetQueryable<T>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task Delete(string id)
    {
        return repository.GetCollection<T>().DeleteOneAsync(x => x.Id == ObjectId.Parse(id));
    }

    public Task Delete(ObjectId id)
    {
        return repository.GetCollection<T>().DeleteOneAsync(x => x.Id == id);
    }
}