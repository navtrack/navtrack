using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Navtrack.DataAccess.Model;

namespace Navtrack.DataAccess.Migrations.SqlServer
{
    public class SqlServerDesignTimeDbContextFactory : IDesignTimeDbContextFactory<NavtrackContext>
    {
        public NavtrackContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<NavtrackContext> optionsBuilder =
                new DbContextOptionsBuilder<NavtrackContext>();

            optionsBuilder.UseSqlServer(
                "data source=localhost;initial catalog=navtrack;user id=navtrack;password=navtrack;",
                b => b.MigrationsAssembly(GetType().Assembly.FullName));

            return new NavtrackContext(optionsBuilder.Options);
        }
    }
}