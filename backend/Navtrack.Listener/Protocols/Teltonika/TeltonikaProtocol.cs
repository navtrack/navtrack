using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Teltonika;

[Service(typeof(IProtocol))]
public class TeltonikaProtocol : BaseProtocol
{
    public override int Port => 7002;

    public override int? GetMessageLength(byte[] buffer, int bytesReadCount)
    {
        if (bytesReadCount > 7 && buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0 && buffer[3] == 0)
        {
            const int prefixLength = 4;
            const int dataFieldLength = 4;
            uint dataLength = buffer[4..8].ToUInt4();
            const int crcLength = 4;
            
            int totalLength = prefixLength + dataFieldLength + (int)dataLength + crcLength;

            return totalLength;
        }

        return base.GetMessageLength(buffer, bytesReadCount);
    }
}