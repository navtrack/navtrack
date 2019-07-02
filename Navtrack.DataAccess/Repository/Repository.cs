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

        public Repository(IDbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
            
            dbContext = dbContextFactory.CreateDbContext();
        }

        public IQueryable<T> GetEntities<T>() where T : class
        {
            return dbContext.Set<T>().AsNoTracking();
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork(dbContextFactory.CreateDbContext());
        }
    }
}