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
                length = ReadMessage(networkStream, buffer, client.Protocol.DetectNewLine, cancellationToken);

                if (length > 0)
                {
                    logger.LogDebug($"{client.Protocol}: received {length} bytes");

                    await messageHandler.HandleMessage(client, networkStream, buffer[..length]);
                }
            } while (!cancellationToken.IsCancellationRequested && length > 0 && networkStream.CanRead);
        }

        private static int ReadMessage(INetworkStreamWrapper networkStream, byte[] buffer, bool detectNewLine,
            CancellationToken cancellationToken)
        {
            if (detectNewLine)
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
                         networkStream.CanRead &&
                         oneByteBuffer[0] != 0x0A && bytesReadCount < 2048);

                return bytesReadCount;
            }

            return networkStream.Read(buffer, 0, 2048);
        }
    }
}