using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Navtrack.Common.Model;
using Navtrack.Common.Services;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Listener.Services.Protocols.Teltonika
{
    [Service(typeof(IProtocol))]
    public class TeltonikaProtocol : IProtocol
    {
        private readonly ITeltonikaLocationParser teltonikaLocationParser;
        private readonly ILocationService locationService;
        private readonly IMapper mapper;

        public TeltonikaProtocol(ITeltonikaLocationParser teltonikaLocationParser, IMapper mapper, ILocationService locationService)
        {
            this.teltonikaLocationParser = teltonikaLocationParser;
            this.mapper = mapper;
            this.locationService = locationService;
        }

        public async Task HandleClient(TcpClient client, CancellationToken stoppingToken)
        {
            await using NetworkStream networkStream = client.GetStream();
            using BinaryReader binaryReader = new BinaryReader(networkStream);
            await using (BinaryWriter binaryWriter = new BinaryWriter(networkStream))
            {
                byte[] data = new byte[2048];
                bool firstDataReceived = true;

                string imei = string.Empty;

                while (binaryReader.Read() != -1)
                {
                    binaryReader.Read(data, 0, data.Length);

                    if (firstDataReceived)
                    {
                        for (int i = 1; i <= 15; i++)
                        {
                            imei += (char)data[i];
                        }

                        binaryWriter.Write(01);
                        firstDataReceived = false;
                    }
                    else
                    {
                        List<Location> locations = teltonikaLocationParser.Convert(data, imei).ToList();

                        if (locations.Any())
                        {
                            await locationService.AddRange(locations);

                            string reply = "000000";

                            if (true)
                            {
                                reply += Utility.ConvertToHex(locations.Count);
                            }
                            else
                            {
                                reply += Utility.ConvertToHex(0);
                            }

                            binaryWriter.Write(Utility.HexStringToByteArray(reply));
                        }
                    }
                }

                binaryWriter.Close();
            }

            binaryReader.Close();
            networkStream.Close();
        }

        public int Port => 6802;
    }
}