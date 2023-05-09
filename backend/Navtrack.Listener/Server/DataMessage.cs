using System.Text;
using Navtrack.Listener.Helpers;

namespace Navtrack.Listener.Server;

public class DataMessage
{
    public DataMessage(byte[] bytes, string splitMessageBy)
    {
        Bytes = bytes;
        string[] hex = HexUtil.ConvertByteArrayToHexStringArray(Bytes);
        Hex = hex;
        HexString = string.Join("", Hex);
        String = Encoding.ASCII.GetString(Bytes);
        CommaSplit = String.Split(",");
        BarSplit = String.Split("|");
        Reader = new MessageReader(String);
        ByteReader = new ByteReader(bytes, hex);
        Split = string.IsNullOrEmpty(splitMessageBy) ? CommaSplit : String.Split(splitMessageBy);
    }

    public byte[] Bytes { get; }
    public string[] Hex { get; }
    public string HexString { get; }
    public string String { get; }
    public string[] CommaSplit { get; }
    public string[] BarSplit { get; }
    public MessageReader Reader { get; }
    public ByteReader ByteReader { get; }
    public string[] Split { get; set; }
}