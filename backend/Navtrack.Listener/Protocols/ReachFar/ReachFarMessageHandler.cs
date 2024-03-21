using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.ReachFar;

[Service(typeof(ICustomMessageHandler<ReachFarProtocol>))]
public class ReachFarMessageHandler : BaseTkStarMessageHandler<ReachFarProtocol>;