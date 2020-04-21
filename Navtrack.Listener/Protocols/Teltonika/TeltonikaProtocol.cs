using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Teltonika
{
    [Service(typeof(IProtocol))]
    public class TeltonikaProtocol : IProtocol
    {
        public int Port => 7002;
        public int[] AdditionalPorts => new[] {6802};
    }
}