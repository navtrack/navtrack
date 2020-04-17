using System.IO;
using System.Threading.Tasks;
using Navtrack.Library.DI;
using Navtrack.Listener.Models;
using Navtrack.Listener.Services;

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
        public int[] AdditionalPorts => new[] {7001};

        public async Task HandleStream(ProtocolInput protocolInput)
        {
            using StreamReader streamReader = new StreamReader(protocolInput.NetworkStream);

            while (!protocolInput.CancellationToken.IsCancellationRequested)
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