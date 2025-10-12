using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Arknav;

[Service(typeof(IProtocol))]
public class ArknavProtocol : BaseProtocol
{
    public override short Port => 7031;
}