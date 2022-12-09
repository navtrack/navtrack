using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.ReachFar;

[Service(typeof(IProtocol))]
public class ReachFarProtocol : TkStarProtocol
{
    public override int Port => 7026;
}