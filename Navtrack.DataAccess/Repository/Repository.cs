using System.Linq;
using Microsoft.EntityFrameworkCore;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Repository
{
    [Service(typeof(IRepository))]
    public class Repository : IRepository
    {
        private readonly IDbContextFactory dbContextFactory;
        private readonly DbContext dbContext;
        private readonly IInterceptorService interceptorService;

        public Repository(IDbContextFactory dbContextFactory, IInterceptorService interceptorService)
        {
            this.dbContextFactory = dbContextFactory;
            this.interceptorService = interceptorService;

            dbContext = dbContextFactory.CreateDbContext();
        }

        public IQueryable<T> GetEntities<T>() where T : class
        {
            return dbContext.Set<T>().AsNoTracking();
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork(dbContextFactory.CreateDbContext(), interceptorService);
        }
    }
}