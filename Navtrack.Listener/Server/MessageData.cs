using System.Text;
using Navtrack.Listener.Helpers;

namespace Navtrack.Listener.Server
{
    public class MessageData
    {
        public MessageData(byte[] bytes)
        {
            Bytes = bytes;
            Hex = HexUtil.ConvertByteArrayToHexStringArray(Bytes);
            String = Encoding.ASCII.GetString(Bytes);
            StringSplit = String.Split(",");
            Reader = new MessageReader(String);
        }

        public byte[] Bytes { get; }
        public string[] Hex { get; }
        public string String { get; }
        public string[] StringSplit { get; }
        public MessageReader Reader { get; }
    }
}