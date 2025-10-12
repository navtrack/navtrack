using System.Collections.Generic;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.VSun;

[Service(typeof(IProtocol))]
public class VSunProtocol : BaseProtocol
{
    public override short Port => 7043;
    public override IEnumerable<byte[]> MessageEnd => new[] {new byte[] {0x0D, 0x0A}};
}