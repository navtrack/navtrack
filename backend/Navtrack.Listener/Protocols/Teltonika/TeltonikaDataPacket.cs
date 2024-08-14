namespace Navtrack.Listener.Protocols.Teltonika;

internal class TeltonikaDataPacket
{
    public short Id { get; set; }
    public byte[] Value { get; set; }
    public bool? Error { get; set; }
    public string Hex { get; set; }
    public bool Parsed { get; set; }
}