using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Navtelecom;

[Service(typeof(IProtocol))]
public class NavtelecomProtocol : BaseProtocol
{
    public override short Port => 7054;
}