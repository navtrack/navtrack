using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Totem;

[Service(typeof(IProtocol))]
public class TotemProtocol : BaseProtocol
{
    public override int Port => 7005;
    public override byte[] MessageStart => [0x24, 0x24];
    public override int? GetMessageLength(byte[] buffer, int bytesReadCount)
    {
        if (buffer.Length >= 6)
        {
            int? headerIndex = buffer.GetStartIndex(MessageStart);

            if (headerIndex.HasValue)
            {
                int lengthStartIndex = headerIndex.Value + 2; // 2 = header
                int lengthEndIndex = lengthStartIndex + 4; // 4 = length 

                if (int.TryParse(StringUtil.ConvertByteArrayToString(buffer[lengthStartIndex..lengthEndIndex]),
                        out int length))
                {
                    return length; 
                }
            }
        }

        return null;
    }
}