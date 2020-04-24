using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;

namespace Navtrack.Core
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IWebHost host = CreateWebHostBuilder(args).Build();
            
            await MigrateDatabase(host);

            await host.RunAsync();
        }

        private static async Task MigrateDatabase(IWebHost host)
        {
            IServiceScope serviceScope = host.Services.CreateScope();
            
            await serviceScope.ServiceProvider
                .GetService<IDbContextFactory>()
                .CreateDbContext()
                .Database
                .MigrateAsync();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(Bootstrapper.ConfigureServices)
                .UseStartup<Startup>();
    }
}