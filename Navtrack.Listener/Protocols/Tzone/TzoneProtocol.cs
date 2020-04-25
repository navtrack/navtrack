using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Tzone
{
    [Service(typeof(IProtocol))]
    public class TzoneProtocol : BaseProtocol
    {
        public override int Port => 7006;
        public override byte[] MessageStart => new byte[] {0x24, 0x24};
        public override byte[] MessageEnd => new byte[] {0x0D, 0x0A};
    }
}