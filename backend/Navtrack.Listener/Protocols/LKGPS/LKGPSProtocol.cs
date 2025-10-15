using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.LKGPS;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class LKGPSProtocol : TkStarProtocol
{
    public override short Port => 7015;
}