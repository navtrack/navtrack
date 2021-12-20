using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Navtrack.Library.DI;

namespace Navtrack.Listener.Services;

[Service(typeof(IHostedService), ServiceLifetime.Singleton)]
public class ListenerHostedService : BackgroundService
{
    private readonly IServiceProvider serviceProvider;

    public ListenerHostedService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        IServiceScope serviceScope = serviceProvider.CreateScope();
        IListenerService listenerService = serviceScope.ServiceProvider.GetService<IListenerService>();
            
        await listenerService.Execute(cancellationToken);
    }
}