using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Satellite;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class SatelliteProtocol : BaseProtocol
{
    public override short Port => 7059;
}