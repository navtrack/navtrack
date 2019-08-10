using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Navtrack.Common.Services;
using Navtrack.Library.DI;

namespace Navtrack.Listener.Protocols.Meitrack
{
    [Service(typeof(IProtocol))]
    public class MeitrackProtocol : IProtocol
    {
        private readonly IMeitrackLocationParser meitrackLocationParser;
        private readonly ILocationService locationService;

        public MeitrackProtocol(IMeitrackLocationParser meitrackLocationParser, ILocationService locationService)
        {
            this.meitrackLocationParser = meitrackLocationParser;
            this.locationService = locationService;
        }

        public async Task HandleClient(TcpClient client, CancellationToken stoppingToken)
        {
            using (StreamReader streamReader = new StreamReader(client.GetStream()))
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    string line = await streamReader.ReadLineAsync();

                    MeitrackLocation location = meitrackLocationParser.Parse(line);

                    if (location != null)
                    {
                        await locationService.Add(location);
                    }
                }

                streamReader.Close();
            }

            client.Close();
        }

        public int Port => 6801;
    }
}