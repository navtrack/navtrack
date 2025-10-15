using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.iTracGPS;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
// ReSharper disable once InconsistentNaming
public class iTracGPSProtocol : TkStarProtocol
{
    public override short Port => 7028;
}