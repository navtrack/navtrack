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
public class ListenerHostedService(IProtocolListener protocolListener, IEnumerable<IProtocol> protocols) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        List<IProtocol> orderedProtocols = protocols.OrderBy(x => x.Port).ToList();

        foreach (IProtocol protocol in orderedProtocols)
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