using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Haicom;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class HaicomProtocol : BaseProtocol
{
    public override short Port => 7032;
    public override byte[] MessageStart => [0x24];
}