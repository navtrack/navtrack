using System.Threading.Tasks;

namespace Navtrack.Listener.Protocols
{
    public interface IProtocol
    {
        Task HandleStream(ProtocolInput protocolInput);
        int Port { get; }
        int[] AdditionalPorts => new int[0]; // TODO temporary, will be removed
    }
}