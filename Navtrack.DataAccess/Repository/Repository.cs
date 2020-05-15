using System.Linq;
using Microsoft.EntityFrameworkCore;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Repository
{
    [Service(typeof(IRepository))]
    public class Repository : IRepository
    {
        private readonly ICustomDbContextFactory customDbContextFactory;
        private readonly DbContext dbContext;
        private readonly IInterceptorService interceptorService;

        public Repository(ICustomDbContextFactory customDbContextFactory, IInterceptorService interceptorService)
        {
            this.customDbContextFactory = customDbContextFactory;
            this.interceptorService = interceptorService;

            dbContext = customDbContextFactory.CreateDbContext();
        }

        public IQueryable<T> GetEntities<T>() where T : class
        {
            return dbContext.Set<T>().AsNoTracking();
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork(customDbContextFactory.CreateDbContext(), interceptorService);
        }
    }
}