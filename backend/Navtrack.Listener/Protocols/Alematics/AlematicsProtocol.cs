using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Alematics;

[Service(typeof(IProtocol))]
public class AlematicsProtocol : BaseProtocol
{
    public override int Port => 7029;
    public override byte[] MessageStart => [0x24];
}