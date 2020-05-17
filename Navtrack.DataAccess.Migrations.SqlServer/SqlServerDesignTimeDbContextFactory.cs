using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Navtrack.DataAccess.Model;

namespace Navtrack.DataAccess.Migrations.SqlServer
{
    public class SqlServerDesignTimeDbContextFactory : IDesignTimeDbContextFactory<NavtrackDbContext>
    {
        public NavtrackDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<NavtrackDbContext> optionsBuilder =
                new DbContextOptionsBuilder<NavtrackDbContext>();

            optionsBuilder.UseSqlServer(
                "data source=localhost;initial catalog=navtrack;user id=navtrack;password=navtrack;",
                b => b.MigrationsAssembly(GetType().Assembly.FullName));

            return new NavtrackDbContext(optionsBuilder.Options);
        }
    }
}