using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Repository
{
    [Service(typeof(IConnectionStringProvider))]
    internal class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IConfiguration configuration;

        public ConnectionStringProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ApplyConnectionString<T>(DbContextOptionsBuilder<T> optionsBuilder) where T : DbContext
        {
            ConnectionString connectionString = GetConnectionString();

            switch (connectionString.DatabaseType)
            {
                case DatabaseType.SqlServer:
                    optionsBuilder.UseSqlServer(connectionString.Value,
                        x => x.MigrationsAssembly(
                            
                            typeof(Migrations.SqlServer.Reference).Assembly.FullName));
                    break;
                case DatabaseType.PostgreSql:
                    optionsBuilder.UseNpgsql(connectionString.Value,
                        x => x.MigrationsAssembly(
                            typeof(Migrations.PostgresSql.Reference).Assembly.FullName));
                    break;
                case DatabaseType.MySql:
                    optionsBuilder.UseMySQL(connectionString.Value,
                        x => x.MigrationsAssembly(
                            typeof(Migrations.MySql.Reference).Assembly.FullName));
                    break;
                case DatabaseType.Sqlite:
                    optionsBuilder.UseSqlite(connectionString.Value,
                        x => x.MigrationsAssembly(
                            typeof(Migrations.Sqlite.Reference).Assembly.FullName));
                    break;
            }
        }

        private ConnectionString GetConnectionString()
        {
            foreach (DatabaseType databaseType in Enum.GetValues(typeof(DatabaseType)).Cast<DatabaseType>())
            {
                string connectionString = configuration.GetConnectionString(databaseType.ToString());

                if (!string.IsNullOrEmpty(connectionString))
                {
                    return new ConnectionString
                    {
                        DatabaseType = databaseType,
                        Value = connectionString
                    };
                }
            }

            throw new Exception("You must set a connection string.");
        }
    }
}