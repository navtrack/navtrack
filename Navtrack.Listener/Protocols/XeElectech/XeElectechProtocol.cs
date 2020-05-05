using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.SinoTrack;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.XeElectech
{
    [Service(typeof(IProtocol))]
    public class XeElectechProtocol : SinoTrackProtocol
    {
        public override int Port => 7019;
    }
}