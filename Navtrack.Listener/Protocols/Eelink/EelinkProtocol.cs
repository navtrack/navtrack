using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Eelink;

[Service(typeof(IProtocol))]
public class EelinkProtocol : BaseProtocol
{
    public override int Port => 7021;
    public override byte[] MessageStart => new byte[] { 0x67, 0x67 };
}