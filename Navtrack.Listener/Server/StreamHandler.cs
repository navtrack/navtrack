using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Server;

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
        byte[] buffer = new byte[ServerVariables.BufferLength];
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

    private static int ReadMessage(INetworkStreamWrapper networkStream, byte[] buffer, IProtocol protocol,
        CancellationToken cancellationToken)
    {
        int bytesReadCount = 0;
        byte[] oneByteBuffer = new byte[1];
        int length;

        int? messageLength = null;

        do
        {
            length = networkStream.Read(oneByteBuffer, 0, 1);

            if (length > 0)
            {
                buffer[bytesReadCount++] = oneByteBuffer[0];

                messageLength = protocol.GetMessageLength(buffer[..bytesReadCount]);
            }
        } while (!cancellationToken.IsCancellationRequested &&
                 networkStream.CanRead &&
                 networkStream.DataAvailable &&
                 length > 0 &&
                 bytesReadCount < ServerVariables.BufferLength &&
                 (!messageLength.HasValue || bytesReadCount < messageLength) &&
                 !ReachedEnd(buffer, bytesReadCount, protocol));

        return bytesReadCount;
    }

    private static int GetStartIndex(byte[] buffer, in int length, IProtocol protocol)
    {
        if (protocol.MessageStart.Length > 0)
        {
            for (int i = 0; i < length-protocol.MessageStart.Length; i++)
            {
                int endIndex = i + protocol.MessageStart.Length;

                if (protocol.MessageStart.IsEqual(buffer[i..endIndex]))
                {
                    return i;
                }
            }
        }

        return 0;
    }

    private static bool ReachedEnd(byte[] buffer, int bytesReadCount, IProtocol protocol)
    {
        return protocol.MessageEnd.Where(x => x.Length > 0).Any(bytes =>
        {
            int startIndex = bytesReadCount - bytes.Length;

            return startIndex > 0 && bytes.IsEqual(buffer[startIndex..bytesReadCount]);
        });
    }
}