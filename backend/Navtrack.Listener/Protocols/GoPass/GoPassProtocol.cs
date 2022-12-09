using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.GoPass;

[Service(typeof(IProtocol))]
public class GoPassProtocol : BaseProtocol
{
    public override int Port => 7039;
}