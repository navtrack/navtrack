using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Eelink;

[Service(typeof(IProtocol))]
public class EelinkProtocol : BaseProtocol
{
    public override int Port => 7021;
    public override byte[] MessageStart => [0x67, 0x67];
}