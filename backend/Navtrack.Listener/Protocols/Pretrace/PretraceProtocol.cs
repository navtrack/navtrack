using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Pretrace;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class PretraceProtocol : BaseProtocol
{
    public override short Port => 7030;
    public override byte[] MessageStart => [0x24];
}