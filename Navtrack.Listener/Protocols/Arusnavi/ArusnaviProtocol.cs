using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Arusnavi;

[Service(typeof(IProtocol))]
public class ArusnaviProtocol : BaseProtocol
{
    public override int Port => 7057;
}