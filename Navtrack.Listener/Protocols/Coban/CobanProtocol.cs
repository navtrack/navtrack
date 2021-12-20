using System.Collections.Generic;
using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Coban;

[Service(typeof(IProtocol))]
public class CobanProtocol : BaseProtocol
{
    public override int Port => 7007;
    public override IEnumerable<byte[]> MessageEnd => new List<byte[]> {new byte[] {0x3B}};
}