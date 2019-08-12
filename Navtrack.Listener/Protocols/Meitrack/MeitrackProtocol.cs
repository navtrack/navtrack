using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Navtrack.Common.Model;
using Navtrack.Common.Services;
using Navtrack.DataAccess.Model;
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

        public int Port => 6801;
        
        public async Task HandleStream(ProtocolInput protocolInput)
        {
            using StreamReader streamReader = new StreamReader(protocolInput.NetworkStream);

            while (!protocolInput.StoppingToken.IsCancellationRequested)
            {
                string data = await streamReader.ReadLineAsync();

                Location<MeitrackData> location = meitrackLocationParser.Parse(data);

                if (location != null)
                {
                    await locationService.Add(location);
                }
            }

            streamReader.Close();
        }
    }
}