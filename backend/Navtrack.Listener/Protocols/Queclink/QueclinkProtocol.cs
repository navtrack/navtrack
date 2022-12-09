using System.Collections.Generic;
using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Queclink;

[Service(typeof(IProtocol))]
public class QueclinkProtocol : BaseProtocol
{
    public override int Port => 7008;
    public override IEnumerable<byte[]> MessageEnd => new List<byte[]> {new byte[] {0x24}};
}