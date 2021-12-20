using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Totem;

[Service(typeof(IProtocol))]
public class TotemProtocol : BaseProtocol
{
    public override int Port => 7005;
    public override byte[] MessageStart => new byte[] {0x24, 0x24};
    public override int? GetMessageLength(byte[] bytes)
    {
        if (bytes.Length >= 6)
        {
            int? headerIndex = bytes.GetStartIndex(MessageStart);

            if (headerIndex.HasValue)
            {
                int lengthStartIndex = headerIndex.Value + 2; // 2 = header
                int lengthEndIndex = lengthStartIndex + 4; // 4 = length 

                if (int.TryParse(StringUtil.ConvertByteArrayToString(bytes[lengthStartIndex..lengthEndIndex]),
                        out int length))
                {
                    return length; 
                }
            }
        }

        return null;
    }
}