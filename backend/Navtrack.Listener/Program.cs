using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
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

        DbContextUtils.AddDbContext<NavtrackDbContext>(hostApplicationBuilder.Services,
            hostApplicationBuilder.Configuration);

        hostApplicationBuilder.Services.AddCustomServices<Program>();

        IHost host = hostApplicationBuilder.Build();

        await host.RunAsync();
    }
}