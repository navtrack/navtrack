using System.Linq;
using Microsoft.EntityFrameworkCore;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Repository
{
    [Service(typeof(IRepository))]
    public class Repository : IRepository
    {
        private readonly DbContext dbContext;

        public Repository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<T> GetEntities<T>() where T : class
        {
            return dbContext.Set<T>().AsNoTracking();
        }
    }
}