using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.GoPass;

[Service(typeof(IProtocol))]
public class GoPassProtocol : BaseProtocol
{
    public override short Port => 7039;
}