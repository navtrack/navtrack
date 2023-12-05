using Navtrack.Listener.Helpers;
using static System.String;

namespace Navtrack.Listener.Protocols.Meiligao;

public class MeiligaoOutputMessage(MeiligaoCommands command, string[] deviceId, string data)
{
    private const string PacketHeaderHex = "4040";
    private const string PacketEndHex = "0D0A";

    public string PacketBody => $"{PacketHeaderHex}{PacketLength:X4}{Join(Empty, deviceId)}{(int) command:X2}{data}";

    public string PacketBodyWithChecksum => $"{PacketBody}{Checksum}{PacketEndHex}";

    // 2 (header) + 2 (length) + 7 (device id) + 2 (command) + X (data) + 2 (checksum) + 2 (footer)
    public int PacketLength => 2 + 2 + 7 + 2 + data.Length / 2 + 2 + 2;

    public string Checksum => $"{Crc16Util.Ccitt(HexUtil.ConvertHexStringToByteArray(PacketBody)):X4}";
}