using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.XeElectech;

[Service(typeof(ICustomMessageHandler<XeElectechProtocol>))]
public class XeElectechMessageHandler : BaseTkStarMessageHandler<XeElectechProtocol>
{
}