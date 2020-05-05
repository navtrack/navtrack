using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.SinoTrack;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.XeElectech
{
    [Service(typeof(ICustomMessageHandler<XeElectechProtocol>))]
    public class XeElectechMessageHandler : SinoTrackMessageHandler
    {
    }
}