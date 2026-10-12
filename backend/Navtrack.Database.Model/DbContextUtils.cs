using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Database.Interceptors;

namespace Navtrack.Database.Model;

public static class DbContextUtils
{
    public static void AddDbContext<T>(IServiceCollection services, IConfigurationManager configurationManager,
        bool enableRetryOnFailure = false)
        where T : BaseNavtrackDbContext
    {
        string? connectionString = configurationManager.GetConnectionString("Postgres");
        if (!string.IsNullOrEmpty(connectionString))
        {
            AddDbContext<T>(services, connectionString, enableRetryOnFailure);
        }
    }

    public static void AddDbContext<T>(IServiceCollection services, string connectionString,
        bool enableRetryOnFailure = false)
        where T : BaseNavtrackDbContext
    {
        if (!string.IsNullOrEmpty(connectionString))
        {
            services.AddScoped<EntitySaveChangesInterceptor>();

            services.AddDbContext<DbContext, T>((serviceProvider, dbContextOptions) =>
                dbContextOptions
                    .UseNpgsql(connectionString, postgresOptions =>
                    {
                        postgresOptions.UseNetTopologySuite().ConfigureDataSource(dataSourceBuilder =>
                        {
                            dataSourceBuilder.EnableDynamicJson();
                        });

                        if (enableRetryOnFailure)
                        {
                            postgresOptions.EnableRetryOnFailure();
                        }
                    })
                    .AddInterceptors(serviceProvider.GetRequiredService<EntitySaveChangesInterceptor>()));
        }
    }
}
