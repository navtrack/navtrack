using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Totem
{
    [Service(typeof(IProtocol))]
    public class TotemProtocol : BaseProtocol
    {
        public override int Port => 7005;
        public override byte[] MessageStart => new byte[] {0x24, 0x24};
    }
}