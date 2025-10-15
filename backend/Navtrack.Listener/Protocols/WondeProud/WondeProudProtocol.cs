using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.WondeProud;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class WondeProudProtocol : BaseProtocol
{
    public override short Port => 7046;
}