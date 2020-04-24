using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Navtrack.Library.DI;

namespace Navtrack.Listener.Server
{
    [Service(typeof(IStreamHandler))]
    public class StreamHandler : IStreamHandler
    {
        private readonly ILogger<StreamHandler> logger;
        private readonly IMessageHandler messageHandler;

        public StreamHandler(ILogger<StreamHandler> logger, IMessageHandler messageHandler)
        {
            this.logger = logger;
            this.messageHandler = messageHandler;
        }

        public async Task HandleStream(CancellationToken cancellationToken, Client client,
            INetworkStreamWrapper networkStream)
        {
            byte[] buffer = new byte[2048];
            int length;

            do
            {
                length = ReadMessage(networkStream, buffer, client.Protocol, cancellationToken);

                if (length > 0)
                {
                    logger.LogDebug($"{client.Protocol}: received {length} bytes");

                    int startIndex = GetStartIndex(buffer, length, client.Protocol);

                    await messageHandler.HandleMessage(client, networkStream, buffer[startIndex..length]);
                }
            } while (!cancellationToken.IsCancellationRequested && length > 0 && networkStream.CanRead);
        }

        private int GetStartIndex(byte[] buffer, in int length, IProtocol clientProtocol)
        {
            if (clientProtocol.MessageStart.Length > 0)
            {
                for (int i = 0; i < length - clientProtocol.MessageStart.Length + 1; i++)
                {
                    bool equal = true;
                    for (int j = 0; j < clientProtocol.MessageStart.Length; j++)
                    {
                        if (buffer[i + j] != clientProtocol.MessageStart[j])
                        {
                            equal = false;
                        }
                    }

                    if (equal)
                    {
                        return i;
                    }
                }
            }

            return 0;
        }

        private static int ReadMessage(INetworkStreamWrapper networkStream, byte[] buffer, IProtocol protocol,
            CancellationToken cancellationToken)
        {
            if (protocol.DetectNewLine)
            {
                int bytesReadCount = 0;
                byte[] oneByteBuffer = new byte[1];
                int length;

                do
                {
                    length = networkStream.Read(oneByteBuffer, 0, 1);
                    if (length > 0)
                    {
                        buffer[bytesReadCount++] = oneByteBuffer[0];
                    }
                } while (!cancellationToken.IsCancellationRequested &&
                         length > 0 &&
                         networkStream.CanRead && !ReachedEnd(buffer, bytesReadCount, protocol) &&
                         bytesReadCount < 2048);

                return bytesReadCount;
            }

            return networkStream.Read(buffer, 0, 2048);
        }

        private static bool ReachedEnd(byte[] buffer, int bytesReadCount, IProtocol protocol)
        {
            for (int i = 0; i < protocol.MessageEnd.Length; i++)
            {
                int bufferIndex = bytesReadCount - protocol.MessageEnd.Length + i;

                if (buffer[bufferIndex] != protocol.MessageEnd[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}