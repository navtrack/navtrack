using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.SinoTrack;

[Service(typeof(ICustomMessageHandler<SinoTrackProtocol>))]
public class SinoTrackMessageHandler : BaseTkStarMessageHandler<SinoTrackProtocol>
{
}