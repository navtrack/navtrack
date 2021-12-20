using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Navtelecom;

[Service(typeof(IProtocol))]
public class NavtelecomProtocol : BaseProtocol
{
    public override int Port => 7054;
}