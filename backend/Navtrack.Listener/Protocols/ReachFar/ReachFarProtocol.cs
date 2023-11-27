using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.ReachFar;

[Service(typeof(IProtocol))]
public class ReachFarProtocol : TkStarProtocol
{
    public override int Port => 7026;
}