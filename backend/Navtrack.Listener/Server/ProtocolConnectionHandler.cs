using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Server;

[Service(typeof(IProtocolConnectionHandler))]
public class ProtocolConnectionHandler(ILogger<ProtocolConnectionHandler> logger, IProtocolMessageHandler handler)
    : IProtocolConnectionHandler
{
    public async Task HandleConnection(ProtocolConnectionContext connectionContext, CancellationToken cancellationToken)
    {
        logger.LogDebug("{Protocol}: connected {EndPoint}", connectionContext.Protocol,
            connectionContext.NetworkStream.RemoteEndPoint);

        byte[] buffer = new byte[ServerVariables.BufferLength];
        int length;

        do
        {
            length = ReadMessage(connectionContext.NetworkStream, buffer, connectionContext.Protocol,
                cancellationToken);

            if (length > 0)
            {
                logger.LogDebug("{ClientProtocol}: received {Length} bytes", connectionContext.Protocol, length);

                int startIndex = GetStartIndex(buffer, length, connectionContext.Protocol);

                await handler.HandleMessage(connectionContext, connectionContext.NetworkStream,
                    buffer[startIndex..length]);
            }
        } while (!cancellationToken.IsCancellationRequested && length > 0 && connectionContext.NetworkStream.CanRead);

        logger.LogDebug("{ClientProtocol}: disconnected {EndPoint}", connectionContext.Protocol,
            connectionContext.NetworkStream.RemoteEndPoint);

        await connectionContext.DisposeAsync();
    }

    private static int ReadMessage(INetworkStreamWrapper networkStream, byte[] buffer, IProtocol protocol,
        CancellationToken cancellationToken)
    {
        int totalBytesReadCount = 0;
        bool readOneByte;
        int? messageLength = null;

        do
        {
            byte[] oneByteBuffer = new byte[1];

            readOneByte = networkStream.Read(oneByteBuffer, 0, 1) > 0;

            if (readOneByte)
            {
                buffer[totalBytesReadCount++] = oneByteBuffer[0];

                messageLength ??= protocol.GetMessageLength(buffer, totalBytesReadCount);
            }
        } while (!cancellationToken.IsCancellationRequested &&
                 networkStream is { CanRead: true, DataAvailable: true } &&
                 readOneByte &&
                 totalBytesReadCount < ServerVariables.BufferLength &&
                 (!messageLength.HasValue || totalBytesReadCount < messageLength) &&
                 !ReachedEnd(buffer, totalBytesReadCount, protocol));

        return totalBytesReadCount;
    }

    private static int GetStartIndex(byte[] buffer, in int length, IProtocol protocol)
    {
        if (protocol.MessageStart.Length > 0)
        {
            for (int i = 0; i < length - protocol.MessageStart.Length; i++)
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