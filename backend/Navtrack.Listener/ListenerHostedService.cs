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
public class ListenerHostedService(IProtocolListener protocolListener, IEnumerable<IProtocol> protocols) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        List<IProtocol> orderedProtocols = protocols.OrderBy(x => x.Port).ToList();

        if (orderedProtocols.Count == 0)
        {
            return;
        }

        using CancellationTokenSource cancellation = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
        List<Task> listeners = orderedProtocols.Select(protocol => protocolListener.Start(protocol, cancellation.Token)).ToList();

        try
        {
            Task completed = await Task.WhenAny(listeners);
            await completed;
        }
        finally
        {
            await cancellation.CancelAsync();
            await Task.WhenAll(listeners);
        }
    }
}
