using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Ruptela;

[Service(typeof(IProtocol))]
public class RuptelaProtocol : BaseProtocol
{
    public override int Port => 7056;
}