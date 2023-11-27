using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.VjoyCar;

[Service(typeof(ICustomMessageHandler<VjoyCarProtocol>))]
public class VjoyCarMessageHandler : BaseVjoyCarMessageHandler<VjoyCarProtocol>
{
}