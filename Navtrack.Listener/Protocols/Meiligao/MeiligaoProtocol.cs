using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Meiligao
{
    [Service(typeof(IProtocol))]
    public class MeiligaoProtocol : BaseProtocol
    {
        public override int Port => 7003;
        public override byte[] MessageStart => new byte[] {0x24, 0x24};
        public override byte[] MessageEnd => new byte[] {0x0D, 0x0A};
    }
}