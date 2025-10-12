using System.Collections.Generic;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.KingSword;

[Service(typeof(IProtocol))]
public class KingSwordProtocol : BaseProtocol
{
    public override short Port => 7034;
    public override byte[] MessageStart => [0x2A];
    public override IEnumerable<byte[]> MessageEnd => new[] {new byte[] {0x23}};
}