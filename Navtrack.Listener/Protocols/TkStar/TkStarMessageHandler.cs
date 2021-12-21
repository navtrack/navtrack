using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.TkStar;

[Service(typeof(ICustomMessageHandler<TkStarProtocol>))]
public class TkStarMessageHandler : BaseTkStarMessageHandler<TkStarProtocol>
{
}