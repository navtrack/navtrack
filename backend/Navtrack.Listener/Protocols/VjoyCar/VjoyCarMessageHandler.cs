using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.VjoyCar;

[Service(typeof(ICustomMessageHandler<VjoyCarProtocol>))]
public class VjoyCarMessageHandler : BaseVjoyCarMessageHandler<VjoyCarProtocol>
{
}