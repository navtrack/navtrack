using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Neomatica;

[Service(typeof(IProtocol))]
public class NeomaticaProtocol : BaseProtocol
{
    public override int Port => 7058;
}