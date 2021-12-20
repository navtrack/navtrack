using System.Collections.Generic;
using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.TkStar;

[Service(typeof(IProtocol))]
public class TkStarProtocol : BaseProtocol
{
    public override int Port => 7011;
    public override byte[] MessageStart => new byte[] {0x2A};
    public override IEnumerable<byte[]> MessageEnd => new List<byte[]> {new byte[] {0x23}};
}