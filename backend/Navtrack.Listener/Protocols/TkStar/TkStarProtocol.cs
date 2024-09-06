using System.Collections.Generic;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.TkStar;

[Service(typeof(IProtocol))]
public class TkStarProtocol : BaseProtocol
{
    public override int Port => 7011;
    public override byte[] MessageStart => [0x2A];
    public override IEnumerable<byte[]> MessageEnd => new List<byte[]> {new byte[] {0x23}};
}