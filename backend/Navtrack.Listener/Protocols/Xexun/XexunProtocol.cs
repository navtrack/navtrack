using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Xexun;

[Service(typeof(IProtocol))]
public class XexunProtocol : BaseProtocol
{
    public override short Port => 7017;
}