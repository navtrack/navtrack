using System.Collections.Generic;
using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.VSun;

[Service(typeof(IProtocol))]
public class VSunProtocol : BaseProtocol
{
    public override int Port => 7043;
    public override IEnumerable<byte[]> MessageEnd => new[] {new byte[] {0x0D, 0x0A}};
}