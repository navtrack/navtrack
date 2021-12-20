using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.CanTrack;

[Service(typeof(IProtocol))]
public class CanTrackProtocol : TkStarProtocol
{
    public override int Port => 7014;
}