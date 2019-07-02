using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Model
{
    [Service(typeof(IDbContextFactory))]
    public class NavtrackDbContextFactory : IDbContextFactory
    {
        public DbContext CreateDbContext()
        {
            DbContextOptionsBuilder<NavtrackContext> optionsBuilder =
                new DbContextOptionsBuilder<NavtrackContext>();
            
            optionsBuilder.UseSqlServer(
                "data source=localhost;initial catalog=navtrack;user id=navtrack;password=navtrack;");

            return new NavtrackContext(optionsBuilder.Options);
        }
    }
}