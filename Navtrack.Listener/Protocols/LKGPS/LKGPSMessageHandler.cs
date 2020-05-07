using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.SinoTrack;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.LKGPS
{
    [Service(typeof(ICustomMessageHandler<LKGPSProtocol>))]
    public class LKGPSMessageHandler : SinoTrackMessageHandler
    {
    }
}