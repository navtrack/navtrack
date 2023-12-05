using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Services;

[Service(typeof(IHostedService), ServiceLifetime.Singleton)]
public class ListenerHostedService(IServiceProvider provider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        IServiceScope serviceScope = provider.CreateScope();
        IListenerService listenerService = serviceScope.ServiceProvider.GetService<IListenerService>();
            
        await listenerService.Execute(cancellationToken);
    }
}