using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Navtelecom;

[Service(typeof(IProtocol))]
public class NavtelecomProtocol : BaseProtocol
{
    public override int Port => 7054;
}