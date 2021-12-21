using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.ReachFar;

[Service(typeof(ICustomMessageHandler<ReachFarProtocol>))]
public class ReachFarMessageHandler : BaseTkStarMessageHandler<ReachFarProtocol>
{
}