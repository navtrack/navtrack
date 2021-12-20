using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Neomatica;

[Service(typeof(IProtocol))]
public class NeomaticaProtocol : BaseProtocol
{
    public override int Port => 7058;
}