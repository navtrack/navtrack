using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Bofan;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class BofanProtocol : BaseProtocol
{
    public override short Port => 7042;
    public override byte[] MessageStart => [0x24];
}