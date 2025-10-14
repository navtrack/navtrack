using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Services.Devices;
using Navtrack.Listener.Models;
using Navtrack.Shared.Library.DI;
using Sentry;

namespace Navtrack.Listener.Server;

[Service(typeof(IProtocolListener))]
public class ProtocolListener(
    ILogger<ProtocolListener> logger,
    IDeviceConnectionRepository deviceConnectionRepository,
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
                ProtocolConnectionContext connectionContext = await GetConnectionContext(protocol, tcpClient);
                
                using IServiceScope scope = serviceProvider.CreateScope();
                IProtocolConnectionHandler protocolConnectionHandler = scope.ServiceProvider.GetRequiredService<IProtocolConnectionHandler>();
              
                _ = protocolConnectionHandler.HandleConnection(connectionContext, cancellationToken);
            }
            catch (Exception exception)
            {
                logger.LogCritical(exception, "{Protocol}", protocol);
                SentrySdk.CaptureException(exception);
            }
        }

        listener.Stop();
    }

    private async Task<ProtocolConnectionContext> GetConnectionContext(IProtocol protocol, TcpClient tcpClient)
    {
        NetworkStreamWrapper networkStream = new(tcpClient);

        DeviceConnectionEntity deviceConnectionDocument = new()
        {
            Port = protocol.Port,
            IPAddress = networkStream.RemoteEndPoint,
            CreatedDate = DateTime.UtcNow
        };
        await deviceConnectionRepository.Add(deviceConnectionDocument);

        ProtocolConnectionContext connectionContext =
            new(networkStream, protocol, deviceConnectionDocument.Id);

        return connectionContext;
    }
}