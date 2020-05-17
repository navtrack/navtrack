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
            DbContextOptionsBuilder<NavtrackDbContext> optionsBuilder =
                new DbContextOptionsBuilder<NavtrackDbContext>();

            connectionStringProvider.ApplyConnectionString(optionsBuilder);

            return new NavtrackDbContext(optionsBuilder.Options);
        }
    }
}