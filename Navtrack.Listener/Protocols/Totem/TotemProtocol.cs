using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Totem
{
    [Service(typeof(IProtocol))]
    public class TotemProtocol : IProtocol
    {
        public int Port => 7005;
    }
}