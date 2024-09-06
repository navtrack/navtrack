using System.Collections.Generic;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Gosafe;

[Service(typeof(IProtocol))]
public class GosafeProtocol : BaseProtocol
{
    public override int Port => 7022;
    public override byte[] MessageStart => [0x2A];
    public override IEnumerable<byte[]> MessageEnd => new[] { [0x23], new byte[] {0x24}};
}