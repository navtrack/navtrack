using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.CanTrack;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class CanTrackProtocol : TkStarProtocol
{
    public override short Port => 7014;
}