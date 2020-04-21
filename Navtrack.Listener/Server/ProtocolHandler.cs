using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Navtrack.Library.DI;

namespace Navtrack.Listener.Server
{
    [Service(typeof(IProtocolHandler))]
    public class ProtocolHandler : IProtocolHandler
    {
        private readonly ILogger<ProtocolHandler> logger;
        private readonly IClientHandler clientHandler;

        public ProtocolHandler(ILogger<ProtocolHandler> logger, IClientHandler clientHandler)
        {
            this.logger = logger;
            this.clientHandler = clientHandler;
        }

        [SuppressMessage("ReSharper", "AssignmentIsFullyDiscarded")]
        public async Task HandleProtocol(CancellationToken cancellationToken, IProtocol protocol, int port)
        {
            TcpListener listener = null;

            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                logger.LogDebug($"{protocol}: listening on {port}");

                while (!cancellationToken.IsCancellationRequested)
                {
                    TcpClient tcpClient = await listener.AcceptTcpClientAsync();
                    logger.LogDebug($"{protocol}: connected {tcpClient.Client.RemoteEndPoint}");
                    
                    Client client = new Client
                    {
                        TcpClient = tcpClient,
                        Protocol = protocol
                    };

                    _ = clientHandler.HandleClient(cancellationToken, client);
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
}