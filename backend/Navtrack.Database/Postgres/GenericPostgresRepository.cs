using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Navtrack.Database.Postgres;

public class GenericPostgresRepository<T> : IGenericPostgresRepository<T> where T : BaseEntity
{
    protected readonly IPostgresRepository repository;

    protected GenericPostgresRepository(IPostgresRepository repository)
    {
        this.repository = repository;
    }

    public virtual Task<List<T>> GetByIds(IEnumerable<Guid> ids)
    {
        return repository.GetQueryable<T>().Where(x => ids.Contains(x.Id)).ToListAsync();
    }

    public virtual Task Add(T entity)
    {
        repository.GetQueryable<T>().Add(entity);

        return repository.GetDbContext().SaveChangesAsync();
    }

    public Task Update(T entity)
    {
        repository.GetQueryable<T>().Update(entity);

        return repository.GetDbContext().SaveChangesAsync();
    }

    public virtual Task AddRange(IEnumerable<T> entities)
    {
        repository.GetQueryable<T>().AddRangeAsync(entities);
        return repository.GetDbContext().SaveChangesAsync();
    }

    public virtual Task<T?> GetById(string? id)
    {
        return !string.IsNullOrEmpty(id)
            ? repository.GetQueryable<T>().FirstOrDefaultAsync(x => x.Id == Guid.Parse(id))
            : Task.FromResult<T?>(null);
    }

    public virtual async Task Delete(T entity)
    {
        repository.GetQueryable<T>().Remove(entity);

        await repository.GetDbContext().SaveChangesAsync();
    }
}