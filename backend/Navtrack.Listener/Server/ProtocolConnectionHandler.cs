using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Server;

[Service(typeof(IProtocolConnectionHandler))]
public class ProtocolConnectionHandler(
    IProtocolMessageHandler handler,
    IProtocolConnectionContextFactory protocolConnectionContextFactory,
    ILogger<ProtocolConnectionHandler> logger)
    : IProtocolConnectionHandler
{
    public async Task HandleConnection(TcpClientAdapter tcpClient, IProtocol protocol,
        CancellationToken cancellationToken)
    {
        await using ProtocolConnectionContext protocolConnectionContext =
            await protocolConnectionContextFactory.GetConnectionContext(protocol, tcpClient);

        logger.LogDebug("{Protocol}: connected {EndPoint}", protocolConnectionContext.Protocol,
            protocolConnectionContext.NetworkStream.RemoteEndPoint);

        try
        {
            byte[] buffer = new byte[ServerVariables.BufferLength];
            int length;

            do
            {
                length = await ReadMessage(protocolConnectionContext.NetworkStream, buffer, protocolConnectionContext.Protocol,
                    cancellationToken);

                if (length > 0)
                {
                    logger.LogDebug("{ClientProtocol}: received {Length} bytes", protocolConnectionContext.Protocol, length);

                    int startIndex = GetStartIndex(buffer, length, protocolConnectionContext.Protocol);

                    await handler.HandleMessage(protocolConnectionContext, protocolConnectionContext.NetworkStream,
                        buffer[startIndex..length]);
                }
            } while (!cancellationToken.IsCancellationRequested && length > 0 && protocolConnectionContext.NetworkStream.CanRead);
        }
        catch (InvalidDataException exception)
        {
            logger.LogWarning("{Protocol}: rejected packet from {EndPoint}: {Reason}", protocolConnectionContext.Protocol,
                protocolConnectionContext.NetworkStream.RemoteEndPoint, exception.Message);
        }

        logger.LogDebug("{ClientProtocol}: disconnected {EndPoint}", protocolConnectionContext.Protocol,
            protocolConnectionContext.NetworkStream.RemoteEndPoint);
    }

    private static async Task<int> ReadMessage(INetworkStreamWrapper networkStream, byte[] buffer, IProtocol protocol,
        CancellationToken cancellationToken)
    {
        int totalBytesReadCount = 0;
        bool readOneByte;
        int? messageLength = null;
        byte[] oneByteBuffer = new byte[1];

        do
        {
            readOneByte = await networkStream.ReadAsync(oneByteBuffer, cancellationToken) > 0;

            if (readOneByte)
            {
                buffer[totalBytesReadCount++] = oneByteBuffer[0];

                messageLength ??= protocol.GetMessageLength(buffer, totalBytesReadCount);

                if (messageLength.HasValue && (messageLength < totalBytesReadCount || messageLength > buffer.Length))
                {
                    throw new InvalidDataException("The declared packet length is invalid or exceeds the buffer size.");
                }
            }
        } while (!cancellationToken.IsCancellationRequested &&
                 networkStream.CanRead &&
                 (protocol.HasMessageLength || messageLength.HasValue || networkStream.DataAvailable) &&
                 readOneByte &&
                 totalBytesReadCount < ServerVariables.BufferLength &&
                 (!messageLength.HasValue || totalBytesReadCount < messageLength) &&
                 !ReachedEnd(buffer, totalBytesReadCount, protocol));

        cancellationToken.ThrowIfCancellationRequested();

        if ((messageLength.HasValue && totalBytesReadCount != messageLength) ||
            (protocol.HasMessageLength && !messageLength.HasValue && totalBytesReadCount > 0))
        {
            throw new InvalidDataException("The connection ended before a complete packet was received.");
        }

        return totalBytesReadCount;
    }

    private static int GetStartIndex(byte[] buffer, in int length, IProtocol protocol)
    {
        if (protocol.MessageStart.Length > 0)
        {
            for (int i = 0; i <= length - protocol.MessageStart.Length; i++)
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

            return startIndex >= 0 && bytes.IsEqual(buffer[startIndex..bytesReadCount]);
        });
    }
}
