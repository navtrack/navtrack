using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Xirgo;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class XirgoProtocol : BaseProtocol
{
    public override short Port => 7024;
    public override byte[] MessageStart => [0x24, 0x24];
    public override IEnumerable<byte[]> MessageEnd => new List<byte[]> {new byte[] {0x23, 0x23}};
}