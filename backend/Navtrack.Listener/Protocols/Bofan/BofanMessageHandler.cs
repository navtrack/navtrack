using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Bofan;

[Service(typeof(ICustomMessageHandler<BofanProtocol>))]
public class BofanMessageHandler : BaseBofanMessageHandler<BofanProtocol>;