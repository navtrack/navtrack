using System.Collections.Generic;
using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Xirgo;

[Service(typeof(IProtocol))]
public class XirgoProtocol : BaseProtocol
{
    public override int Port => 7024;
    public override byte[] MessageStart => new byte[] {0x24, 0x24};
    public override IEnumerable<byte[]> MessageEnd => new List<byte[]> {new byte[] {0x23, 0x23}};
}