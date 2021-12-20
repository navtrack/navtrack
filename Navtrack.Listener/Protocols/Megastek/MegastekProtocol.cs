using System.Collections.Generic;
using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Megastek;

[Service(typeof(IProtocol))]
public class MegastekProtocol : BaseProtocol
{
    public override int Port => 7004;
    public override IEnumerable<byte[]> MessageEnd => new List<byte[]> {new byte[] {0x0D, 0x0A}, new byte[] {0x21}};
}