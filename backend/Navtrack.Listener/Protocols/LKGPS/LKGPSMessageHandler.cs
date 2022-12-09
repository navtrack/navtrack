using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.LKGPS;

[Service(typeof(ICustomMessageHandler<LKGPSProtocol>))]
public class LKGPSMessageHandler : BaseTkStarMessageHandler<LKGPSProtocol>
{
}