namespace Navtrack.Listener.Server
{
    public interface IProtocol
    {
        int Port { get; }
        int[] AdditionalPorts { get; }
        byte[] MessageStart { get; }
        byte[] MessageEnd { get; }
        bool DetectNewLine { get; }
        int? GetLength();
    }
}