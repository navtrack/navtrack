using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Navtrack.DataAccess.Mongo;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener;

public class Program
{
    public static async Task Main(string[] args)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        HostApplicationBuilder hostApplicationBuilder = Host.CreateApplicationBuilder();

        hostApplicationBuilder.Services.AddOptions<MongoOptions>()
            .Bind(hostApplicationBuilder.Configuration.GetSection(nameof(MongoOptions)));
        
        hostApplicationBuilder.Services.AddCustomServices<Program>();
        
        IHost host = hostApplicationBuilder.Build();

        await host.RunAsync();
    }
}