using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Navtrack.DataAccess.Model.Devices.Connections;
using Navtrack.DataAccess.Services.Positions;
using Navtrack.Listener.Models;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Server;

[Service(typeof(IProtocolListener))]
public class ProtocolListener(
    ILogger<ProtocolListener> logger,
    IProtocolConnectionHandler protocolConnectionHandler,
    IConnectionRepository connectionRepository) : IProtocolListener
{
    public async Task Start(IProtocol protocol, CancellationToken cancellationToken)
    {
        TcpListener? listener = null;

        try
        {
            listener = new TcpListener(IPAddress.Any, protocol.Port);
            listener.Start();
            logger.LogDebug("{Protocol}: listening on {ProtocolPort}", protocol, protocol.Port);

            while (!cancellationToken.IsCancellationRequested)
            {
                TcpClient tcpClient = await listener.AcceptTcpClientAsync(cancellationToken);
                ProtocolConnectionContext connectionContext = await GetConnectionContext(protocol, tcpClient);

                _ = protocolConnectionHandler.HandleConnection(connectionContext, cancellationToken);
            }
        }
        catch (Exception exception)
        {
            logger.LogCritical(exception, "{Protocol}", protocol);
        }
        finally
        {
            listener?.Stop();
        }
    }

    private async Task<ProtocolConnectionContext> GetConnectionContext(IProtocol protocol, TcpClient tcpClient)
    {
        NetworkStreamWrapper networkStream = new(tcpClient);
        
        DeviceConnectionDocument deviceConnectionDocument = new()
        {
            ProtocolPort = protocol.Port,
            Ip = networkStream.RemoteEndPoint,
            CreatedDate = DateTime.UtcNow
        };
        await connectionRepository.Add(deviceConnectionDocument);

        ProtocolConnectionContext connectionContext =
            new(networkStream, protocol, deviceConnectionDocument.Id);
        
        return connectionContext;
    }
}