using System;
using System.Collections.Generic;
using System.IO;
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
public class ProtocolListener(IServiceProvider serviceProvider, ILogger<ProtocolListener> logger) : IProtocolListener
{
    public async Task Start(IProtocol protocol, CancellationToken cancellationToken)
    {
        TcpListener listener = new(IPAddress.Any, protocol.Port);
        listener.Start();
        logger.LogDebug("{Protocol}: listening on {ProtocolPort}", protocol, protocol.Port);

        HashSet<Task> connections = [];

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                TcpClient tcpClient = await listener.AcceptTcpClientAsync(cancellationToken);
                connections.RemoveWhere(task => task.IsCompleted);
                connections.Add(HandleConnection(tcpClient, protocol, cancellationToken));
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
        finally
        {
            listener.Stop();
            await Task.WhenAll(connections);
        }
    }

    private async Task HandleConnection(TcpClient tcpClient, IProtocol protocol, CancellationToken cancellationToken)
    {
        using TcpClientAdapter tcpClientAdapter = new(tcpClient);

        try
        {
            await using AsyncServiceScope serviceScope = serviceProvider.CreateAsyncScope();
            IProtocolConnectionHandler protocolConnectionHandler =
                serviceScope.ServiceProvider.GetRequiredService<IProtocolConnectionHandler>();

            await protocolConnectionHandler.HandleConnection(tcpClientAdapter, protocol, cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (InvalidDataException exception)
        {
            logger.LogWarning("{Protocol}: rejected packet from {EndPoint}: {Reason}", protocol,
                tcpClientAdapter.GetRemoteEndPoint(), exception.Message);
        }
        catch (Exception exception) when (exception is IOException or SocketException ||
                                         (exception is ObjectDisposedException && cancellationToken.IsCancellationRequested))
        {
            logger.LogDebug(exception, "{Protocol}: connection closed by {EndPoint}", protocol,
                tcpClientAdapter.GetRemoteEndPoint());
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{Protocol}: failed to handle connection from {EndPoint}", protocol,
                tcpClientAdapter.GetRemoteEndPoint());
            SentrySdk.CaptureException(exception);
        }
    }
}
