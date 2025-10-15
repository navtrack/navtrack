using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.GlobalSat;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class GlobalSatProtocol : BaseProtocol
{
    public override short Port => 7038;
    public override IEnumerable<byte[]> MessageEnd => new[] {new byte[] {0x21}};
}