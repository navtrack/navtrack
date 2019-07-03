using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Navtrack.Library.DI;

namespace Navtrack.Listener
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(Bootstrapper.ConfigureServices);
    }
}