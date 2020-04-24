using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Meiligao
{
    [Service(typeof(IProtocol))]
    public class MeiligaoProtocol : IProtocol
    {
        public int Port => 7003;
        public byte[] MessageStart => new byte[] {0x24, 0x24};
        public byte[] MessageEnd => new byte[] {0x0D, 0x0A};
    }
}