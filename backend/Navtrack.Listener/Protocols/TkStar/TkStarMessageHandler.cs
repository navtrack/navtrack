using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.TkStar;

[Service(typeof(ICustomMessageHandler<TkStarProtocol>))]
public class TkStarMessageHandler : BaseTkStarMessageHandler<TkStarProtocol>
{
}