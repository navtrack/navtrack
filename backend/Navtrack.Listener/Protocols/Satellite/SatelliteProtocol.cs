using System;
using System.Buffers.Binary;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Satellite;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class SatelliteProtocol : BaseProtocol
{
    public override short Port => 7059;
    public override bool HasMessageLength => true;

    public override int? GetMessageLength(byte[] buffer, int bytesReadCount)
    {
        if (bytesReadCount >= 4 && (buffer[2] != 0xbf || buffer[3] != 0x4c))
        {
            throw new InvalidDataException("Invalid Satellite packet header.");
        }

        return bytesReadCount >= 10 ? BinaryPrimitives.ReadUInt16LittleEndian(buffer.AsSpan(8, 2)) + 10 : null;
    }
}
