using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Meitrack
{
    [Service(typeof(IProtocol))]
    public class MeitrackProtocol : IProtocol
    {
        public int Port => 7001;
        public bool DetectNewLine => true;
        public int[] AdditionalPorts => new[] {6801};
    }
}