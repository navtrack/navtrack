using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.VjoyCar;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class VjoyCarProtocol : BaseProtocol
{
    public override short Port => 7020;
    public override byte[] MessageStart => [0x28];
    public override IEnumerable<byte[]> MessageEnd => new List<byte[]> {new byte[] {0x29}};
}