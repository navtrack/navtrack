using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.CanTrack;

[Service(typeof(ICustomMessageHandler<CanTrackProtocol>))]
public class CanTrackMessageHandler : BaseTkStarMessageHandler<CanTrackProtocol>
{
}