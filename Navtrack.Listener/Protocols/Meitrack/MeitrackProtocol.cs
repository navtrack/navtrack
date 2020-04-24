using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Meitrack
{
    [Service(typeof(IProtocol))]
    public class MeitrackProtocol : BaseProtocol
    {
        public override int Port => 7001;
        public override bool DetectNewLine => true;
        public override int[] AdditionalPorts => new[] {6801};
    }
}