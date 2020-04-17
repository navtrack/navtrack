using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Services;

namespace Navtrack.Listener.Protocols.Meiligao
{
    [Service(typeof(IProtocol))]
    public class MeiligaoProtocol : IProtocol
    {
        private readonly ILocationService locationService;
        private readonly IMeiligaoMessageHandler meiligaoMessageHandler;
        private readonly IMapper mapper;

        public MeiligaoProtocol(ILocationService locationService, IMeiligaoMessageHandler meiligaoMessageHandler,
            IMapper mapper)
        {
            this.locationService = locationService;
            this.meiligaoMessageHandler = meiligaoMessageHandler;
            this.mapper = mapper;
        }

        public int Port => 7003;

        public async Task HandleStream(ProtocolInput protocolInput)
        {
            using StreamReader streamReader = new StreamReader(protocolInput.NetworkStream, Encoding.UTF7);
            await using StreamWriter streamWriter = new StreamWriter(protocolInput.NetworkStream, Encoding.UTF7);

            while (!protocolInput.CancellationToken.IsCancellationRequested)
            {
                int[] frame = GetFrame(protocolInput.NetworkStream, protocolInput.CancellationToken);

                if (frame == null)
                {
                    break;
                }

                MeiligaoMessage message = new MeiligaoMessage(frame);
                MeiligaoCommand command = meiligaoMessageHandler.HandleMessage(message);

                if (command != null)
                {
                    await protocolInput.NetworkStream.WriteAsync(
                        HexUtil.ConvertHexStringToByteArray(command.FullReply));
                }

                Location location = mapper.Map<MeiligaoMessage, Location>(message);

                if (location != null)
                {
                    await locationService.Add(location);
                }
            }

            streamReader.Close();
        }

        private static int[] GetFrame(NetworkStream networkStream, CancellationToken cancellationToken)
        {
            if (networkStream.CanRead)
            {
                int[] buffer = new int[256];
                int bytesRead = 0;

                do
                {
                    buffer[bytesRead++] = networkStream.ReadByte();
                } while (networkStream.DataAvailable && !EndBytesRead(buffer, bytesRead) && buffer[bytesRead] != -1);

                if (bytesRead == 1 && buffer[0] == -1)
                {
                    return null;
                }

                int startIndex = GetStartIndex(buffer, bytesRead);

                return buffer[startIndex..bytesRead];
            }

            return null;
        }

        private static int GetStartIndex(IReadOnlyList<int> buffer, int bytesRead)
        {
            for (int i = 0; i < bytesRead - 1; i++)
            {
                if (buffer[i] == 0x40 && buffer[i + 1] == 0x40 ||
                    buffer[i] == 0x24 && buffer[i + 1] == 0x24)
                {
                    return i;
                }
            }

            return 0;
        }

        private static bool EndBytesRead(IReadOnlyList<int> bytes, in int bytesRead)
        {
            return bytesRead > 1 && bytes[bytesRead - 2] == 0x0D && bytes[bytesRead - 1] == 0xDA;
        }
    }
}