using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Navtrack.Database.Model;

public static class DbContextUtils
{
    public static void AddDbContext<T>(IServiceCollection services, IConfigurationManager configurationManager)
        where T : BaseNavtrackDbContext
    {
        AddDbContext<T>(services, configurationManager.GetConnectionString("Postgres")!);
    }

    public static void AddDbContext<T>(IServiceCollection services, string connectionString)
        where T : BaseNavtrackDbContext
    {
        if (!string.IsNullOrEmpty(connectionString))
        {
            services.AddDbContext<DbContext, T>(dbContextOptions =>
                dbContextOptions.UseNpgsql(connectionString, postgresOptions =>
                    postgresOptions.UseNetTopologySuite().ConfigureDataSource(dataSourceBuilder =>
                    {
                        dataSourceBuilder.EnableDynamicJson();
                    })));
        }
    }
}