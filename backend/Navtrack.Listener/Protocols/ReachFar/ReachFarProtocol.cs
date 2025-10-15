using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.ReachFar;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class ReachFarProtocol : TkStarProtocol
{
    public override short Port => 7026;
}