using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Navtrack.Database.Model;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener;

public class Program
{
    public static async Task Main(string[] args)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        HostApplicationBuilder hostApplicationBuilder = Host.CreateApplicationBuilder();

        hostApplicationBuilder.Services.AddDbContext<DbContext, NavtrackDbContext>(opt =>
            opt.UseNpgsql(
                hostApplicationBuilder.Configuration.GetConnectionString("Postgres")));

        hostApplicationBuilder.Services.AddCustomServices<Program>();
        
        IHost host = hostApplicationBuilder.Build();

        await host.RunAsync();
    }
}