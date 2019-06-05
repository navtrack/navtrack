using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Repository
{
    [Service(typeof(IUnitOfWork))]
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext dbContext;

        public UnitOfWork(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add<T>(T entity) where T : class
        {
            dbContext.Add(entity);
        }

        public Task SaveChanges()
        {
            return dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            dbContext?.Dispose();
        }
    }
}