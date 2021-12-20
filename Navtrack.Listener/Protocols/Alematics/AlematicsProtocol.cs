using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Alematics;

[Service(typeof(IProtocol))]
public class AlematicsProtocol : BaseProtocol
{
    public override int Port => 7029;
    public override byte[] MessageStart => new byte[] {0x24};
}