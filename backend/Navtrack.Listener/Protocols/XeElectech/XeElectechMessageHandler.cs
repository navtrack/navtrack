using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.XeElectech;

[Service(typeof(ICustomMessageHandler<XeElectechProtocol>))]
public class XeElectechMessageHandler : BaseTkStarMessageHandler<XeElectechProtocol>;