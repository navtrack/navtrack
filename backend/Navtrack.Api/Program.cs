using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Navtrack.Library.DI;

namespace Navtrack.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        IWebHost host = WebHost.CreateDefaultBuilder(args)
            .ConfigureServices(Bootstrapper.ConfigureServices)
            .UseSentry()
            .UseStartup<Startup>()
            .Build();

        await host.RunAsync();
    }
}