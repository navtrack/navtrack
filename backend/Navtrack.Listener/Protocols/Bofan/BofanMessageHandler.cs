using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Bofan;

[Service(typeof(ICustomMessageHandler<BofanProtocol>))]
public class BofanMessageHandler : BaseBofanMessageHandler<BofanProtocol>
{
}