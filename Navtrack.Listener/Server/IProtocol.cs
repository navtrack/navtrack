namespace Navtrack.Listener.Server
{
    public interface IProtocol
    {
        int Port { get; }
        bool DetectNewLine => false;
        byte[] MessageStart => new byte[0];
        byte[] MessageEnd => DetectNewLine ? new byte[] { 0x0A } : new byte[0];
        int[] AdditionalPorts => new int[0]; // TODO temporary, will be removed
    }
}