using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.LKGPS;

[Service(typeof(ICustomMessageHandler<LKGPSProtocol>))]
public class LKGPSMessageHandler : BaseTkStarMessageHandler<LKGPSProtocol>;