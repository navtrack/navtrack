using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Xexun;

[Service(typeof(IProtocol))]
public class XexunProtocol : BaseProtocol
{
    public override int Port => 7017;
}