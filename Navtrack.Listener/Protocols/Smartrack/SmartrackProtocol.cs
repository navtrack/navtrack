using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.VjoyCar;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Smartrack;

[Service(typeof(IProtocol))]
public class SmartrackProtocol : VjoyCarProtocol
{
    public override int Port => 7025;
}