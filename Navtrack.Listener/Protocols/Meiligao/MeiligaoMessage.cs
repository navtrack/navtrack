using System;
using Navtrack.Listener.Extensions;
using Navtrack.Listener.Helpers;
using static System.String;

namespace Navtrack.Listener.Protocols.Meiligao
{
    public class MeiligaoMessage
    {
        public readonly string[] Hex;
        public readonly int[] Bytes;

        public MeiligaoMessage(int[] frame)
        {
            Bytes = frame;
            Hex = HexUtil.ConvertIntArrayToHexStringArray(Bytes);
        }

        private bool ContainsNewLine => Bytes[^2] == 0x0D && Bytes[^1] == 0x0A;
        
        private string Checksum => ContainsNewLine ? Hex[^4] + Hex[^3] : Hex[^2] + Hex[^1];
        private string ChecksumComputed =>
            Crc16.Ccitt(Bytes[new Range(0, ContainsNewLine ? Hex.Length - 4 : Hex.Length - 2)]).ToString("X4");
        public bool ChecksumValid => Checksum == ChecksumComputed;

        private int DataStartIndex = 13;
        private int DataEndIndex => ContainsNewLine ? Bytes.Length - 5 : Bytes.Length - 3;
        private bool HasData => DataEndIndex > DataStartIndex;
        private string[] DataHex => HasData ? Hex[DataStartIndex..DataEndIndex] : null;
        private int[] DataBytes => HasData ? Bytes[DataStartIndex..DataEndIndex] : null;
        public MeiligaoDataMessage DataMessage =>
            HasData ? new MeiligaoDataMessage(DataBytes) : null;
        
        public string[] DeviceIdHex => Hex[4..11];
        public string DeviceIdTrimmed => Join(Empty, DeviceIdHex).TrimEnd('F');

        public MeiligaoCommands Command => (MeiligaoCommands) ((Bytes[11] << 8) | Bytes[12]);
    }
}