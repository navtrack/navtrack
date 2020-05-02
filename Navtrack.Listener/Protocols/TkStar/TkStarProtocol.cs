using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.SinoTrack;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.TkStar
{
    [Service(typeof(IProtocol))]
    public class TkStarProtocol : SinoTrackProtocol
    {
        public override int Port => 7012;
    }
}