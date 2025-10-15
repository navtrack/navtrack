using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Eelink;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class EelinkProtocol : BaseProtocol
{
    public override short Port => 7021;
    public override byte[] MessageStart => [0x67, 0x67];
}