using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Autofon;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class AutofonProtocol : BaseProtocol
{
    public override short Port => 7060;
    public override bool HasMessageLength => true;

    public override int? GetMessageLength(byte[] buffer, int bytesReadCount)
    {
        if (bytesReadCount == 0)
        {
            return null;
        }

        return (MessageType)buffer[0] switch
        {
            MessageType.LoginV1 => 12,
            MessageType.LocationV1 => 78,
            MessageType.HistoryV1 => 257,
            MessageType.LoginV2 => 19,
            MessageType.LocationV2 => 34,
            _ => throw new InvalidDataException("Unknown Autofon message type.")
        };
    }
}
