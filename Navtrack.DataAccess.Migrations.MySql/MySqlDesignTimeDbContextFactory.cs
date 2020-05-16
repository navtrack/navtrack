using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Navtrack.DataAccess.Model;

namespace Navtrack.DataAccess.Migrations.MySql
{
    public class MySqlDesignTimeDbContextFactory : IDesignTimeDbContextFactory<NavtrackContext>
    {
        public NavtrackContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<NavtrackContext> optionsBuilder =
                new DbContextOptionsBuilder<NavtrackContext>();

            optionsBuilder.UseMySQL(
                "data source=localhost;initial catalog=navtrack;user id=navtrack;password=navtrack;",
                b => b.MigrationsAssembly(GetType().Assembly.FullName));

            return new NavtrackContext(optionsBuilder.Options);
        }
    }
}