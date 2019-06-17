using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Navtrack.Library.DI;

namespace Navtrack.Listener.Services
{
    [Service(typeof(IHostedService), ServiceLifetime.Singleton)]
    public class ListenerHostedService : BackgroundService
    {
         private readonly ILogger<ListenerHostedService> logger;
        private TcpListener listener;

        public ListenerHostedService(ILogger<ListenerHostedService> logger)
        {
            this.logger = logger;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, 1234);

                listener.Start();

                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();

                    HandleClient(client, stoppingToken);
                }
            }
            catch (Exception exception)
            {
                logger.Log(LogLevel.Critical, exception, $"{nameof(ListenerHostedService)}");
            }
            finally
            {
                listener.Stop();
            }
        }


        private async Task HandleClient(TcpClient client, CancellationToken stoppingToken)
        {
            await using (NetworkStream networkStream = client.GetStream())
            using (StreamReader streamReader = new StreamReader(networkStream))
            await using (StreamWriter streamWriter = new StreamWriter(networkStream))
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    string line = await streamReader.ReadLineAsync();

                    if (!string.IsNullOrEmpty(line))
                    {
                        Console.WriteLine(line);

                        await streamWriter.WriteLineAsync(line);
                        await streamWriter.FlushAsync();
                    }
                    else
                    {
                        break;
                    }
                }

                await streamWriter.WriteLineAsync("closing");
                await streamWriter.FlushAsync();

                streamWriter.Close();
                streamReader.Close();
                networkStream.Close();
            }

            client.Close();
        }
    }
}