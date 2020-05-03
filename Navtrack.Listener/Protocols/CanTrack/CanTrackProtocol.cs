using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.SinoTrack;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.CanTrack
{
    [Service(typeof(IProtocol))]
    public class CanTrackProtocol : SinoTrackProtocol
    {
        public override int Port => 7014;
    }
}