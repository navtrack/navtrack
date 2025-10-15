using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Navtrack.Shared.Library.DI;
using Sentry;

namespace Navtrack.Listener.Server;

[Service(typeof(IProtocolListener), ServiceLifetime.Singleton)]
public class ProtocolListener(
    ILogger<ProtocolListener> logger,
    IServiceProvider serviceProvider) : IProtocolListener
{
    public async Task Start(IProtocol protocol, CancellationToken cancellationToken)
    {
        TcpListener listener = new(IPAddress.Any, protocol.Port);
        listener.Start();
        logger.LogDebug("{Protocol}: listening on {ProtocolPort}", protocol, protocol.Port);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                TcpClient tcpClient = await listener.AcceptTcpClientAsync(cancellationToken);

                IServiceScope serviceScope = serviceProvider.CreateScope();

                IProtocolConnectionHandler protocolConnectionHandler =
                    serviceScope.ServiceProvider.GetRequiredService<IProtocolConnectionHandler>();

                _ = protocolConnectionHandler.HandleConnection(protocol, tcpClient, cancellationToken);
            }
            catch (Exception exception)
            {
                logger.LogCritical(exception, "{Protocol}", protocol);
                SentrySdk.CaptureException(exception);
            }
        }

        listener.Stop();
    }
}