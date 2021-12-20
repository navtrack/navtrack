using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Laipac;

[Service(typeof(IProtocol))]
public class LaipacProtocol : BaseProtocol
{
    public override int Port => 7052;
}