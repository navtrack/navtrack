using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Navtrack.Database.Postgres;

public interface IGenericPostgresRepository<T> where T : class
{
    Task<List<T>> GetByIds(IEnumerable<Guid> ids);
    Task Add(T entity);
    Task Update(T entity);
    Task AddRange(IEnumerable<T> entities);
    Task<T?> GetById(string? id);
    Task Delete(T entity);
}