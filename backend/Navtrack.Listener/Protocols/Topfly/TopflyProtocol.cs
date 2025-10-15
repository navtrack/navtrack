using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Topfly;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class TopflyProtocol : BaseProtocol
{
    public override short Port => 7050;
    public override byte[] MessageStart => [0x23, 0x23];
}