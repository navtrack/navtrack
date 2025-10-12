using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.SinoTrack;

[Service(typeof(IProtocol))]
public class SinoTrackProtocol : TkStarProtocol
{
    public override short Port => 7012;
}