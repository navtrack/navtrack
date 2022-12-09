using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.KeSon;

[Service(typeof(IProtocol))]
public class KeSonProtocol : BaseProtocol
{
    public override int Port => 7041;
}