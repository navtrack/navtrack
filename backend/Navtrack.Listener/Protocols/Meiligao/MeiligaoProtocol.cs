using System.Collections.Generic;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Meiligao;

[Service(typeof(IProtocol))]
public class MeiligaoProtocol : BaseProtocol
{
    public override short Port => 7003;
    public override byte[] MessageStart => [0x24, 0x24];
    public override IEnumerable<byte[]> MessageEnd => new List<byte[]> {new byte[] {0x0D, 0x0A}};
}