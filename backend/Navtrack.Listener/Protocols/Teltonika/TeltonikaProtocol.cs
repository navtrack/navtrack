using System;
using System.Buffers.Binary;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Teltonika;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class TeltonikaProtocol : BaseProtocol
{
    public override short Port => 7002;
    public override bool HasMessageLength => true;

    public override int? GetMessageLength(byte[] buffer, int bytesReadCount)
    {
        if (bytesReadCount >= 2)
        {
            ushort imeiLength = BinaryPrimitives.ReadUInt16BigEndian(buffer.AsSpan(0, 2));
            if (imeiLength > 0)
            {
                if (imeiLength != 15)
                {
                    throw new InvalidDataException("Invalid Teltonika IMEI length.");
                }

                return imeiLength + 2;
            }
        }

        if (bytesReadCount >= 4 && (buffer[2] != 0 || buffer[3] != 0))
        {
            throw new InvalidDataException("Invalid Teltonika packet preamble.");
        }

        if (bytesReadCount > 7 && buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0 && buffer[3] == 0)
        {
            const int prefixLength = 4;
            const int dataFieldLength = 4;
            uint dataLength = buffer[4..8].ToUInt4();
            const int crcLength = 4;
            
            if (dataLength > ServerVariables.BufferLength - prefixLength - dataFieldLength - crcLength)
            {
                throw new InvalidDataException("The Teltonika packet exceeds the buffer size.");
            }

            int totalLength = prefixLength + dataFieldLength + (int)dataLength + crcLength;

            return totalLength;
        }

        return base.GetMessageLength(buffer, bytesReadCount);
    }
}
