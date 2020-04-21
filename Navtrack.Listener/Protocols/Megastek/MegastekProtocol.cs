using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Megastek
{
    [Service(typeof(IProtocol))]
    public class MegastekProtocol : IProtocol
    {
        public int Port => 7005;
    }
}