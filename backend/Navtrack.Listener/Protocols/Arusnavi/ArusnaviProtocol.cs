using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Arusnavi;

[Service(typeof(IProtocol))]
public class ArusnaviProtocol : BaseProtocol
{
    public override int Port => 7057;
}