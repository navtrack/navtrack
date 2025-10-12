using System.Collections.Generic;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Gotop;

[Service(typeof(IProtocol))]
public class GotopProtocol : BaseProtocol
{
    public override short Port => 7037;
    public override IEnumerable<byte[]> MessageEnd => new[] {new byte[] {0x23}};
}