using System.Collections.Generic;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Amwell;

[Service(typeof(IProtocol))]
public class AmwellProtocol : BaseProtocol
{
    public override int Port => 7035;
    public override byte[] MessageStart => [0x29, 0x29];
    public override IEnumerable<byte[]> MessageEnd => new List<byte[]> {new byte[] {0x0D}};
}