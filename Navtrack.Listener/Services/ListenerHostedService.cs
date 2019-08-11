using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Navtrack.Common.Services;
using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Listener.Protocols;
// ReSharper disable AssignmentIsFullyDiscarded

namespace Navtrack.Listener.Services
{
    [Service(typeof(IHostedService), ServiceLifetime.Singleton)]
    public class ListenerHostedService : BackgroundService
    {
        private readonly ILogger<ListenerHostedService> logger;
        private readonly IEnumerable<IProtocol> protocols;
        private readonly IConnectionService connectionService;

        public ListenerHostedService(ILogger<ListenerHostedService> logger, IEnumerable<IProtocol> protocols,
            IConnectionService connectionService)
        {
            this.logger = logger;
            this.protocols = protocols;
            this.connectionService = connectionService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            foreach (IProtocol protocol in protocols)
            {
                _ = HandleProtocol(protocol, stoppingToken);
            }

            return Task.CompletedTask;
        }

        private async Task HandleProtocol(IProtocol protocol, CancellationToken stoppingToken)
        {
            TcpListener listener = null;

            try
            {
                listener = new TcpListener(IPAddress.Any, protocol.Port);

                listener.Start();

                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();

                    _ = HandleClient(client, protocol, stoppingToken);
                }
            }
            catch (Exception exception)
            {
                logger.Log(LogLevel.Critical, exception, $"{nameof(ListenerHostedService)}");
            }
            finally
            {
                listener?.Stop();
            }
        }

        private async Task HandleClient(TcpClient tcpClient, IProtocol protocol, CancellationToken stoppingToken)
        {
            Connection connection = null;

            try
            {
                connection = await connectionService.NewConnection((IPEndPoint) tcpClient.Client.RemoteEndPoint);

                await using (NetworkStream networkStream = tcpClient.GetStream())
                {
                    ProtocolInput protocolInput = new ProtocolInput
                    {
                        NetworkStream = networkStream,
                        StoppingToken = stoppingToken
                    };

                    await protocol.HandleStream(protocolInput);

                    networkStream.Close();
                }

                tcpClient.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                if (connection != null)
                {
                    await connectionService.MarkConnectionAsClosed(connection);
                }
            }
        }
    }
}