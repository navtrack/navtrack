using System.Collections.Generic;
using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Gosafe;

[Service(typeof(IProtocol))]
public class GosafeProtocol : BaseProtocol
{
    public override int Port => 7022;
    public override byte[] MessageStart => new byte[] {0x2A};
    public override IEnumerable<byte[]> MessageEnd => new[] {new byte[] {0x23}, new byte[] {0x24}};
}