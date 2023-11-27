using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Arknav;

[Service(typeof(IProtocol))]
public class ArknavProtocol : BaseProtocol
{
    public override int Port => 7031;
}