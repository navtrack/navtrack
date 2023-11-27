using Navtrack.Listener.Protocols.VjoyCar;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Smartrack;

[Service(typeof(ICustomMessageHandler<SmartrackProtocol>))]
public class SmartrackMessageHandler : BaseVjoyCarMessageHandler<SmartrackProtocol>
{
}