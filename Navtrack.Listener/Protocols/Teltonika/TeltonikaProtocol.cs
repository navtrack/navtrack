using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Navtrack.Library.DI;
using Navtrack.Listener.Services;

namespace Navtrack.Listener.Protocols.Teltonika
{
    [Service(typeof(IProtocol))]
    public class TeltonikaProtocol : IProtocol
    {
        private readonly ITeltonikaLocationParser teltonikaLocationParser;
        private readonly ILocationService locationService;

        public TeltonikaProtocol(ITeltonikaLocationParser teltonikaLocationParser, ILocationService locationService)
        {
            this.teltonikaLocationParser = teltonikaLocationParser;
            this.locationService = locationService;
        }

        public async Task HandleStream(ProtocolInput protocolInput)
        {
            using BinaryReader binaryReader = new BinaryReader(protocolInput.NetworkStream);
            await using (BinaryWriter binaryWriter = new BinaryWriter(protocolInput.NetworkStream))
            {
                byte[] data = new byte[2048];
                bool firstDataReceived = true;

                string imei = string.Empty;

                while (binaryReader.Read() != -1 || !protocolInput.StoppingToken.IsCancellationRequested)
                {
                    binaryReader.Read(data, 0, data.Length);

                    if (firstDataReceived)
                    {
                        for (int i = 1; i <= 15; i++)
                        {
                            imei += (char) data[i];
                        }

                        binaryWriter.Write(01);
                        firstDataReceived = false;
                    }
                    else
                    {
                        List<LocationHolder> locations = teltonikaLocationParser.Convert(data, imei).ToList();

                        if (locations.Any())
                        {
                            await locationService.AddRange(locations.Select(x => x.Location).ToList());

                            string reply = "000000" + Utility.ConvertToHex(locations.Count);

                            binaryWriter.Write(Utility.HexStringToByteArray(reply));
                        }
                    }
                }

                binaryWriter.Close();
            }

            binaryReader.Close();
        }

        public int Port => 6802;
    }
}