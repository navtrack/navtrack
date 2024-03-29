using System.Collections.Generic;
using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Carscop;

[Service(typeof(IProtocol))]
public class CarscopProtocol : TkStarProtocol
{
    public override int Port => 7016;
    public override IEnumerable<byte[]> MessageEnd => new List<byte[]> {new byte[] {0x23}, new byte[] {0x5E}};
}