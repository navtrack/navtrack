using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.CanTrack;

[Service(typeof(ICustomMessageHandler<CanTrackProtocol>))]
public class CanTrackMessageHandler : BaseTkStarMessageHandler<CanTrackProtocol>
{
}