using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.SinoTrack;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Carscop
{
    [Service(typeof(IProtocol))]
    public class CarscopProtocol : SinoTrackProtocol
    {
        public override int Port => 7016;
    }
}