using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Jointech;

[Service(typeof(IProtocol))]
public class JointechProtocol : BaseProtocol
{
    public override int Port => 7040;
}