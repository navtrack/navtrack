using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Navtrack.Common.Services;
using Navtrack.Library.DI;

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

        public int Port => 6802;
        
        public async Task HandleStream(NetworkStream networkStream, CancellationToken stoppingToken)
        {
            using BinaryReader binaryReader = new BinaryReader(networkStream);
            await using (BinaryWriter binaryWriter = new BinaryWriter(networkStream))
            {
                List<byte[]> allData = new List<byte[]>();
                byte[] data = new byte[2048];
                bool firstDataReceived = true;

                string imei = string.Empty;

                if (allData.Count > 10000)
                {
                    allData.Clear();
                }
                
                while (binaryReader.Read() != -1)
                {
                    binaryReader.Read(data, 0, data.Length);
                    allData.Add(data);

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
                            LocationHolder first = locations.First();
                            //first.ProtocolData.Input = allData.ToArray();
                            first.Location.ProtocolData = JsonSerializer.Serialize(first.ProtocolData);
                            
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
    }
}