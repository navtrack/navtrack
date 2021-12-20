using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Ruptela;

[Service(typeof(IProtocol))]
public class RuptelaProtocol : BaseProtocol
{
    public override int Port => 7056;
}