using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Ruptela;

[Service(typeof(IProtocol))]
public class RuptelaProtocol : BaseProtocol
{
    public override short Port => 7056;
}