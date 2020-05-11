using System.Collections.Generic;
using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Gotop
{
    [Service(typeof(IProtocol))]
    public class GotopProtocol : BaseProtocol
    {
        public override int Port => 7037;
        public override byte[] MessageStart => new byte[] {0x23};
        public override IEnumerable<byte[]> MessageEnd => new[] {new byte[] {0x23}};
    }
}