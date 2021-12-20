using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Teltonika;

[Service(typeof(IProtocol))]
public class TeltonikaProtocol : BaseProtocol
{
    public override int Port => 7002;
}