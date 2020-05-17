using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Navtrack.DataAccess.Model;

namespace Navtrack.DataAccess.Migrations.PostgreSql
{
    public class PostgreSqlDesignTimeDbContextFactory : IDesignTimeDbContextFactory<NavtrackDbContext>
    {
        public NavtrackDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<NavtrackDbContext> optionsBuilder =
                new DbContextOptionsBuilder<NavtrackDbContext>();

            optionsBuilder.UseNpgsql(
                "Host=localhost;Database=navtrack;Username=postgres;Password=postgres",
                b => b.MigrationsAssembly(GetType().Assembly.FullName));

            return new NavtrackDbContext(optionsBuilder.Options);
        }
    }
}