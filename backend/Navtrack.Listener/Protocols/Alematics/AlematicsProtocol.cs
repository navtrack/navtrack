using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Alematics;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class AlematicsProtocol : BaseProtocol
{
    public override short Port => 7029;
    public override byte[] MessageStart => [0x24];
}