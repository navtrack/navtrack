using System.Collections.Generic;
using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Eview;

[Service(typeof(IProtocol))]
public class EviewProtocol : BaseProtocol
{
    public override int Port => 7048;
    public override byte[] MessageStart => new byte[] {0x21};
    public override IEnumerable<byte[]> MessageEnd => new[] {new byte[] {0x3B}};
}