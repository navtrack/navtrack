using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.KeSon;

[Service(typeof(IProtocol))]
public class KeSonProtocol : BaseProtocol
{
    public override int Port => 7041;
}