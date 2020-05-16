using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess
{
    [Service(typeof(IDbContextFactory))]
    // ReSharper disable once InconsistentNaming
    public class _NavtrackDbContextFactory : IDbContextFactory
    {
        private readonly IConnectionStringProvider connectionStringProvider;

        public _NavtrackDbContextFactory(IConnectionStringProvider connectionStringProvider)
        {
            this.connectionStringProvider = connectionStringProvider;
        }

        public DbContext CreateDbContext()
        {
            DbContextOptionsBuilder<NavtrackContext> optionsBuilder =
                new DbContextOptionsBuilder<NavtrackContext>();

            connectionStringProvider.ApplyConnectionString(optionsBuilder);

            return new NavtrackContext(optionsBuilder.Options);
        }
    }
}