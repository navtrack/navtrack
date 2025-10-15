using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Coban;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class CobanProtocol : BaseProtocol
{
    public override short Port => 7007;
    public override IEnumerable<byte[]> MessageEnd => new List<byte[]> {new byte[] {0x3B}};
}