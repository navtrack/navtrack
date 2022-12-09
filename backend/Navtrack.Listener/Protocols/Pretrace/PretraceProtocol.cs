using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Pretrace;

[Service(typeof(IProtocol))]
public class PretraceProtocol : BaseProtocol
{
    public override int Port => 7030;
    public override byte[] MessageStart => new byte[] {0x24};
}