using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Navtrack.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext dbContext;
        private readonly IInterceptorService interceptorService;

        public UnitOfWork(DbContext dbContext, IInterceptorService interceptorService)
        {
            this.dbContext = dbContext;
            this.interceptorService = interceptorService;
        }

        public void Add<T>(T entity) where T : class
        {
            dbContext.Add(entity);
        }

        public void AddRange<T>(IEnumerable<T> entities) where T : class
        {
            dbContext.AddRange(entities);
        }

        public Task SaveChanges()
        {
            interceptorService.InterceptChanges(dbContext);
            
            return dbContext.SaveChangesAsync();
        }

        public void Update<T>(T entity) where T : class
        {
            dbContext.Update(entity);
        }

        public IQueryable<T> GetEntities<T>() where T : class
        {
            return dbContext.Set<T>();
        }

        public void Delete<T>(T entity) where T : class
        {
            dbContext.Remove(entity);
        }

        public void Dispose()
        {
            dbContext?.Dispose();
        }
    }
}