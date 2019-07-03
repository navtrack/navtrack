using Microsoft.EntityFrameworkCore.Design;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;

namespace Navtrack.DataAccess.Migrations
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<NavtrackContext>
    {
        private readonly IDbContextFactory dbContextFactory;

        public DesignTimeDbContextFactory(IDbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public NavtrackContext CreateDbContext(string[] args)
        {
            return (NavtrackContext) dbContextFactory.CreateDbContext();
        }
    }
}