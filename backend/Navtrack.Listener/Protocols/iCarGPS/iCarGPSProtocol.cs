using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.iCarGPS;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
// ReSharper disable once InconsistentNaming
public class iCarGPSProtocol : TkStarProtocol
{
    public override short Port => 7027;
}