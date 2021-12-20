using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Arknav;

[Service(typeof(IProtocol))]
public class ArknavProtocol : BaseProtocol
{
    public override int Port => 7031;
}