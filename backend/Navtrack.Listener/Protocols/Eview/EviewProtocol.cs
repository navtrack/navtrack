using System.Collections.Generic;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Eview;

[Service(typeof(IProtocol))]
public class EviewProtocol : BaseProtocol
{
    public override short Port => 7048;
    public override byte[] MessageStart => [0x21];
    public override IEnumerable<byte[]> MessageEnd => new[] {new byte[] {0x3B}};
}