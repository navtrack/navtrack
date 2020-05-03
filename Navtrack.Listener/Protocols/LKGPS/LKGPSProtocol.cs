using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.SinoTrack;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.LKGPS
{
    [Service(typeof(IProtocol))]
    public class LKGPSProtocol : SinoTrackProtocol
    {
        public override int Port => 7015;
    }
}