using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.SinoTrack;

[Service(typeof(ICustomMessageHandler<SinoTrackProtocol>))]
public class SinoTrackMessageHandler : BaseTkStarMessageHandler<SinoTrackProtocol>;