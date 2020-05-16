using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Navtrack.DataAccess.Model;

namespace Navtrack.DataAccess.Migrations.PostgreSql
{
    public class PostgreSqlDesignTimeDbContextFactory : IDesignTimeDbContextFactory<NavtrackContext>
    {
        public NavtrackContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<NavtrackContext> optionsBuilder =
                new DbContextOptionsBuilder<NavtrackContext>();

            optionsBuilder.UseNpgsql(
                "Host=localhost;Database=navtrack;Username=postgres;Password=postgres",
                b => b.MigrationsAssembly(GetType().Assembly.FullName));

            return new NavtrackContext(optionsBuilder.Options);
        }
    }
}