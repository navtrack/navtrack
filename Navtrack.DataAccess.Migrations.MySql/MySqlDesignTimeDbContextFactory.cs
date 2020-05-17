using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Navtrack.DataAccess.Model;

namespace Navtrack.DataAccess.Migrations.MySql
{
    public class MySqlDesignTimeDbContextFactory : IDesignTimeDbContextFactory<NavtrackDbContext>
    {
        public NavtrackDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<NavtrackDbContext> optionsBuilder =
                new DbContextOptionsBuilder<NavtrackDbContext>();

            optionsBuilder.UseMySQL(
                "data source=localhost;initial catalog=navtrack;user id=navtrack;password=navtrack;",
                b => b.MigrationsAssembly(GetType().Assembly.FullName));

            return new NavtrackDbContext(optionsBuilder.Options);
        }
    }
}