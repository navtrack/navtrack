using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.VjoyCar;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Smartrack;

[Service(typeof(ICustomMessageHandler<SmartrackProtocol>))]
public class SmartrackMessageHandler : BaseVjoyCarMessageHandler<SmartrackProtocol>
{
}