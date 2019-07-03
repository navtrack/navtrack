using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Navtrack.DataAccess.Model;

namespace Navtrack.DataAccess.Migrations
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<NavtrackContext>
    {
        private readonly IConfiguration configuration;

        public DesignTimeDbContextFactory(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public NavtrackContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<NavtrackContext> optionsBuilder =
                new DbContextOptionsBuilder<NavtrackContext>();

            string connectionString = configuration.GetConnectionString("navtrack");
            
            optionsBuilder.UseSqlServer(connectionString);

            return new NavtrackContext(optionsBuilder.Options);
        }
    }
}