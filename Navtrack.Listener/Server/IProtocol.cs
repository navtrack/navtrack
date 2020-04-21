namespace Navtrack.Listener.Server
{
    public interface IProtocol
    {
        int Port { get; }
        bool DetectNewLine => false;
        int[] AdditionalPorts => new int[0]; // TODO temporary, will be removed
    }
}