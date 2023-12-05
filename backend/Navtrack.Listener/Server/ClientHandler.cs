using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Navtrack.Listener.Models;
using Navtrack.Listener.Services;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Server;

[Service(typeof(IClientHandler))]
public class ClientHandler(
    IConnectionService service,
    IStreamHandler handler,
    ILogger<ClientHandler> logger)
    : IClientHandler
{
    public async Task HandleClient(CancellationToken cancellationToken, Client client)
    {
        string endPoint = client.TcpClient.Client.RemoteEndPoint?.ToString();
        client.DeviceConnection = await service.NewConnection(endPoint, client.Protocol.Port);

        try
        {
            await using INetworkStreamWrapper networkStream = new NetworkStreamWrapper(client.TcpClient.GetStream());

            await handler.HandleStream(cancellationToken, client, networkStream);

            networkStream.Close();
        }
        catch (Exception exception)
        {
            logger.LogCritical(exception, $"{client.Protocol}");
        }
        finally
        {
            logger.LogDebug($"{client.Protocol}: disconnected {endPoint}");
                
            client.TcpClient.Close();

            await service.MarkConnectionAsClosed(client.DeviceConnection);
        }
    }
}