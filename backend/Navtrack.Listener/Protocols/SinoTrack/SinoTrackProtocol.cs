using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.SinoTrack;

[Service(typeof(IProtocol))]
public class SinoTrackProtocol : TkStarProtocol
{
    public override int Port => 7012;
}