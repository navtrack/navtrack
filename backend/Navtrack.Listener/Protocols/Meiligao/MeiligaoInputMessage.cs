using System;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using static System.String;

namespace Navtrack.Listener.Protocols.Meiligao;

public class MeiligaoInputMessage
{
    public readonly DataMessage DataMessage;

    public MeiligaoInputMessage(DataMessage frame)
    {
        DataMessage = frame;
    }

    private bool ContainsNewLine => DataMessage.Bytes[^2] == 0x0D && DataMessage.Bytes[^1] == 0x0A;
        
    public string Checksum => ContainsNewLine ? DataMessage.Hex[^4] + DataMessage.Hex[^3] : DataMessage.Hex[^2] + DataMessage.Hex[^1];
    public string ChecksumComputed =>
        Crc16Util.Ccitt(DataMessage.Bytes[new Range(0, ContainsNewLine ? DataMessage.Bytes.Length - 4 : DataMessage.Bytes.Length - 2)]).ToString("X4");
    public bool ChecksumValid => Checksum == ChecksumComputed;

    public const int DataStartIndex = 13;
    public int DataEndIndex => ContainsNewLine ? DataMessage.Bytes.Length - 5 : DataMessage.Bytes.Length - 3;
    public bool HasData => DataEndIndex > DataStartIndex;
    public string[] DataHex => HasData ? DataMessage.Hex[DataStartIndex..DataEndIndex] : null;
    public byte[] DataBytes => HasData ? DataMessage.Bytes[DataStartIndex..DataEndIndex] : null;
    public MeiligaoDataMessage MeiligaoDataMessage => HasData ? new MeiligaoDataMessage(DataBytes) : null;
        
    public string[] DeviceIdHex => DataMessage.Hex[4..11];
    public string DeviceIdTrimmed => Join(Empty, DeviceIdHex).TrimEnd('F');

    public MeiligaoCommands Command => (MeiligaoCommands) ((DataMessage.Bytes[11] << 8) | DataMessage.Bytes[12]);
}