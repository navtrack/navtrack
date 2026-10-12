using System;
using System.Buffers.Binary;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Galileosky;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class GalileoskyProtocol : BaseProtocol
{
    public override short Port => 7055;
    public override bool HasMessageLength => true;

    public override int? GetMessageLength(byte[] buffer, int bytesReadCount)
    {
        if (bytesReadCount > 0 && buffer[0] != 0x01)
        {
            throw new InvalidDataException("Invalid Galileosky packet header.");
        }

        return bytesReadCount >= 3
            ? (BinaryPrimitives.ReadUInt16LittleEndian(buffer.AsSpan(1, 2)) & 0x7fff) + 5
            : null;
    }
}
