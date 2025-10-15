using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Megastek;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class MegastekProtocol : BaseProtocol
{
    public override short Port => 7004;
    public override IEnumerable<byte[]> MessageEnd => new List<byte[]> {new byte[] {0x0D, 0x0A}, new byte[] {0x21}};
}