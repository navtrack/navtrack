using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Megastek
{
    [Service(typeof(IProtocol))]
    public class MegastekProtocol : BaseProtocol
    {
        public override int Port => 7004;
    }
}