using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Bofan;

[Service(typeof(IProtocol))]
public class BofanProtocol : BaseProtocol
{
    public override int Port => 7042;
    public override byte[] MessageStart => new byte[] {0x24};
}