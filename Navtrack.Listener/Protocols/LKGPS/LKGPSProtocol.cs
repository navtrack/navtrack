using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.LKGPS;

[Service(typeof(IProtocol))]
public class LKGPSProtocol : TkStarProtocol
{
    public override int Port => 7015;
}