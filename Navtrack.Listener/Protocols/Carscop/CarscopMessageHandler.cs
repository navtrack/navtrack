using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.SinoTrack;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Carscop
{
    [Service(typeof(ICustomMessageHandler<CarscopProtocol>))]
    public class CarscopMessageHandler : SinoTrackMessageHandler
    {
    }
}