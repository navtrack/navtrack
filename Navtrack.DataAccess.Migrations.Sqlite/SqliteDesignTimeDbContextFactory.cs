using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Navtrack.DataAccess.Model;

namespace Navtrack.DataAccess.Migrations.Sqlite
{
    public class SqliteDesignTimeDbContextFactory : IDesignTimeDbContextFactory<NavtrackDbContext>
    {
        public NavtrackDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<NavtrackDbContext> optionsBuilder =
                new DbContextOptionsBuilder<NavtrackDbContext>();

            optionsBuilder.UseSqlite(
                "data source=localhost;initial catalog=navtrack;user id=navtrack;password=navtrack;",
                b => b.MigrationsAssembly(GetType().Assembly.FullName));

            return new NavtrackDbContext(optionsBuilder.Options);
        }
    }
}