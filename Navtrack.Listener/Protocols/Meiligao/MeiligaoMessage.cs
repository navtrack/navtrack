using System;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using static System.String;

namespace Navtrack.Listener.Protocols.Meiligao
{
    public class MeiligaoMessage
    {
        public readonly MessageData MessageData;

        public MeiligaoMessage(MessageData frame)
        {
            MessageData = frame;
        }

        private bool ContainsNewLine => MessageData.Bytes[^2] == 0x0D && MessageData.Bytes[^1] == 0x0A;
        
        private string Checksum => ContainsNewLine ? MessageData.Hex[^4] + MessageData.Hex[^3] : MessageData.Hex[^2] + MessageData.Hex[^1];
        private string ChecksumComputed =>
            Crc16Util.Ccitt(MessageData.Bytes[new Range(0, ContainsNewLine ? MessageData.Bytes.Length - 4 : MessageData.Bytes.Length - 2)]).ToString("X4");
        public bool ChecksumValid => Checksum == ChecksumComputed;

        private const int DataStartIndex = 13;
        private int DataEndIndex => ContainsNewLine ? MessageData.Bytes.Length - 5 : MessageData.Bytes.Length - 3;
        private bool HasData => DataEndIndex > DataStartIndex;
        private string[] DataHex => HasData ? MessageData.Hex[DataStartIndex..DataEndIndex] : null;
        private byte[] DataBytes => HasData ? MessageData.Bytes[DataStartIndex..DataEndIndex] : null;
        public MeiligaoDataMessage MeiligaoDataMessage =>
            HasData ? new MeiligaoDataMessage(DataBytes) : null;
        
        public string[] DeviceIdHex => MessageData.Hex[4..11];
        public string DeviceIdTrimmed => Join(Empty, DeviceIdHex).TrimEnd('F');

        public MeiligaoCommands Command => (MeiligaoCommands) ((MessageData.Bytes[11] << 8) | MessageData.Bytes[12]);
    }
}