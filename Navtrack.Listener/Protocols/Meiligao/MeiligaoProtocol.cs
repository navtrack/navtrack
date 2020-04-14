using System.IO;
using System.Text;
using System.Threading.Tasks;
using Navtrack.Library.DI;
using Navtrack.Listener.Models;
using Navtrack.Listener.Services;

namespace Navtrack.Listener.Protocols.Meiligao
{
    [Service(typeof(IProtocol))]
    public class MeiligaoProtocol : IProtocol
    {
        private readonly IMeiligaoLocationParser meiligaoLocationParser;
        private readonly ILocationService locationService;

        public MeiligaoProtocol(IMeiligaoLocationParser meiligaoLocationParser, ILocationService locationService)
        {
            this.meiligaoLocationParser = meiligaoLocationParser;
            this.locationService = locationService;
        }

        public int Port => 6803;
        
        public async Task HandleStream(ProtocolInput protocolInput)
        {
            using StreamReader streamReader = new StreamReader(protocolInput.NetworkStream, Encoding.UTF7);
            
            while (!protocolInput.CancellationToken.IsCancellationRequested)
            {
                string data = await streamReader.ReadLineAsync();
                
                Location<MeiligaoData> location = meiligaoLocationParser.Parse(data);
                
                if (location != null)
                {
                    await locationService.Add(location);
                }
            }

            streamReader.Close();
        }
    }
}