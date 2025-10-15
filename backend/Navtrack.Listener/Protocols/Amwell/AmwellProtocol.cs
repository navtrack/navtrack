using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Amwell;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class AmwellProtocol : BaseProtocol
{
    public override short Port => 7035;
    public override byte[] MessageStart => [0x29, 0x29];
    public override IEnumerable<byte[]> MessageEnd => new List<byte[]> {new byte[] {0x0D}};
}