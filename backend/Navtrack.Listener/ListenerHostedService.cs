using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener;

[Service(typeof(IHostedService), ServiceLifetime.Singleton)]
public class ListenerHostedService : IHostedService
{
    private readonly IServiceScopeFactory serviceScopeFactory;

    public ListenerHostedService(IServiceScopeFactory serviceScopeFactory)
    {
        this.serviceScopeFactory = serviceScopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();

        IProtocolListener protocolListener = scope.ServiceProvider.GetRequiredService<IProtocolListener>();
        List<IProtocol> protocols = scope.ServiceProvider.GetServices<IProtocol>().OrderBy(x => x.Port).ToList();

        foreach (IProtocol protocol in protocols)
        {
            _ = protocolListener.Start(protocol, cancellationToken);
        }

        await Task.Delay(Timeout.Infinite, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}