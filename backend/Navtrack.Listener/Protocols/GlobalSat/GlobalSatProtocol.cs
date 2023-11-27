using System.Collections.Generic;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.GlobalSat;

[Service(typeof(IProtocol))]
public class GlobalSatProtocol : BaseProtocol
{
    public override int Port => 7038;
    public override IEnumerable<byte[]> MessageEnd => new[] {new byte[] {0x21}};
}