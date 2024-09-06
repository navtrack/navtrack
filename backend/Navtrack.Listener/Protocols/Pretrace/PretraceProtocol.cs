using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Pretrace;

[Service(typeof(IProtocol))]
public class PretraceProtocol : BaseProtocol
{
    public override int Port => 7030;
    public override byte[] MessageStart => [0x24];
}