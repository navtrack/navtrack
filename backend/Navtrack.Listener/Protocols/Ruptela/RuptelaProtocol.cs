using System;
using System.Buffers.Binary;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Ruptela;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class RuptelaProtocol : BaseProtocol
{
    public override short Port => 7056;
    public override bool HasMessageLength => true;

    public override int? GetMessageLength(byte[] buffer, int bytesReadCount)
    {
        return bytesReadCount >= 2 ? BinaryPrimitives.ReadUInt16BigEndian(buffer.AsSpan(0, 2)) + 4 : null;
    }
}
