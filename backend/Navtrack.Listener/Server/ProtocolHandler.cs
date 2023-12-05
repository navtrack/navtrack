using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Navtrack.Listener.Models;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Server;

[Service(typeof(IProtocolHandler))]
public class ProtocolHandler(ILogger<ProtocolHandler> logger, IClientHandler handler)
    : IProtocolHandler
{
    [SuppressMessage("ReSharper", "AssignmentIsFullyDiscarded")]
    public async Task HandleProtocol(CancellationToken cancellationToken, IProtocol protocol)
    {
        TcpListener listener = null;

        try
        {
            listener = new TcpListener(IPAddress.Any, protocol.Port);
            listener.Start();
            logger.LogDebug($"{protocol}: listening on {protocol.Port}");

            while (!cancellationToken.IsCancellationRequested)
            {
                TcpClient tcpClient = await listener.AcceptTcpClientAsync();
                logger.LogDebug($"{protocol}: connected {tcpClient.Client.RemoteEndPoint}");
                    
                Client client = new()
                {
                    TcpClient = tcpClient,
                    Protocol = protocol
                };

                _ = handler.HandleClient(cancellationToken, client);
            }
        }
        catch (Exception exception)
        {
            logger.LogCritical(exception, $"{protocol}");
        }
        finally
        {
            listener?.Stop();
        }
    }
}