using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Database.Interceptors;

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
            services.AddScoped<EntitySaveChangesInterceptor>();

            services.AddDbContext<DbContext, T>((serviceProvider, dbContextOptions) =>
                dbContextOptions
                    .UseNpgsql(connectionString, postgresOptions =>
                        postgresOptions.UseNetTopologySuite().ConfigureDataSource(dataSourceBuilder =>
                        {
                            dataSourceBuilder.EnableDynamicJson();
                        }))
                    .AddInterceptors(serviceProvider.GetRequiredService<EntitySaveChangesInterceptor>()));
        }
    }
}
