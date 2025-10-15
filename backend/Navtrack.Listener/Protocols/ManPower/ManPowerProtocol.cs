using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.ManPower;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class ManPowerProtocol : BaseProtocol
{
    public override short Port => 7045;
}