using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Navtrack.Library.DI;
using Navtrack.Library.Extensions;

namespace Navtrack.DataAccess.Repository
{
    [Service(typeof(IInterceptorService))]
    public class InterceptorService : IInterceptorService
    {
        private readonly IEnumerable<IInterceptor> interceptors;

        public InterceptorService(IEnumerable<IInterceptor> interceptors)
        {
            this.interceptors = interceptors;
        }

        public void InterceptChanges(DbContext dbContext)
        {
            List<EntityEntry> changedEntities = dbContext.ChangeTracker.Entries().ToList();

            foreach (EntityEntry entityEntry in changedEntities)
            {
                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (entityEntry.State == EntityState.Added)
                {
                    interceptors.ForEach(x => x.OnAdd(entityEntry.Entity));
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    interceptors.ForEach(x => x.OnUpdate(entityEntry.Entity));
                }
                else if (entityEntry.State == EntityState.Deleted)
                {
                    interceptors.ForEach(x => x.OnDelete(entityEntry.Entity));
                }
            }
        }
    }
}