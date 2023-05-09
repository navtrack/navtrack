using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Navtrack.DataAccess.Mongo;
using Navtrack.Library.DI;

namespace Navtrack.Listener;

public class Program
{
    public static async Task Main(string[] args)
    {
        Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
        
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureLogging((_, builder) =>
            {
                builder.AddSentry();
            })
            .ConfigureServices((hostContext, collection) =>
            {
                IConfiguration configuration = hostContext.Configuration;
                collection.AddOptions<MongoOptions>().Bind(configuration.GetSection(nameof(MongoOptions)));

                Bootstrapper.ConfigureServices<Program>(collection);
            })
            .Build();

        await host.RunAsync();
    }
}