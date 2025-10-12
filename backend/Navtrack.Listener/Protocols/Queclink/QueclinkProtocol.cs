using System.Collections.Generic;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Queclink;

[Service(typeof(IProtocol))]
public class QueclinkProtocol : BaseProtocol
{
    public override short Port => 7008;
    public override IEnumerable<byte[]> MessageEnd => new List<byte[]> {new byte[] {0x24}};
}