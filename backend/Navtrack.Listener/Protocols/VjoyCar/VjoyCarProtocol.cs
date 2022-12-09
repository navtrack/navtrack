using System.Collections.Generic;
using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.VjoyCar;

[Service(typeof(IProtocol))]
public class VjoyCarProtocol : BaseProtocol
{
    public override int Port => 7020;
    public override byte[] MessageStart => new byte[] { 0x28 };
    public override IEnumerable<byte[]> MessageEnd => new List<byte[]> {new byte[] {0x29}};
}