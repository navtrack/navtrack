using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Listener.Services;

namespace Navtrack.Listener.Server
{
    [Service(typeof(IClientHandler))]
    public class ClientHandler : IClientHandler
    {
        private readonly IConnectionService connectionService;
        private readonly IStreamHandler streamHandler;
        private readonly ILogger<ClientHandler> logger;

        public ClientHandler(IConnectionService connectionService, IStreamHandler streamHandler,
            ILogger<ClientHandler> logger)
        {
            this.connectionService = connectionService;
            this.streamHandler = streamHandler;
            this.logger = logger;
        }

        public async Task HandleClient(CancellationToken cancellationToken, Client client)
        {
            ConnectionEntity connection = await connectionService.NewConnection((IPEndPoint) client.TcpClient.Client.RemoteEndPoint);

            try
            {
                await using INetworkStreamWrapper networkStream = new NetworkStreamWrapper(client.TcpClient.GetStream());

                await streamHandler.HandleStream(cancellationToken, client, networkStream);

                networkStream.Close();
            }
            catch (Exception exception)
            {
                logger.LogCritical(exception, $"{client.Protocol}");
            }
            finally
            {
                logger.LogDebug($"{client.Protocol}: disconnected {client.TcpClient.Client.RemoteEndPoint}");
                
                client.TcpClient.Close();

                await connectionService.MarkConnectionAsClosed(connection);
            }
        }
    }
}