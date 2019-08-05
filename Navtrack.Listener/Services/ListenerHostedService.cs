using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Navtrack.Library.DI;
using Navtrack.Listener.Services.Protocols;

namespace Navtrack.Listener.Services
{
    [Service(typeof(IHostedService), ServiceLifetime.Singleton)]
    public class ListenerHostedService : BackgroundService
    {
        private readonly ILogger<ListenerHostedService> logger;
        private readonly IEnumerable<IProtocol> protocols;

        public ListenerHostedService(ILogger<ListenerHostedService> logger, IEnumerable<IProtocol> protocols)
        {
            this.logger = logger;
            this.protocols = protocols;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            foreach (IProtocol protocol in protocols)
            {
                TcpListener listener = null;
                
                try
                {
                    listener = new TcpListener(IPAddress.Any, protocol.Port);

                    listener.Start();

                    while (true)
                    {
                        TcpClient client = await listener.AcceptTcpClientAsync();

                        protocol.HandleClient(client, stoppingToken);
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
        }
    }
}