using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.XeElectech;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class XeElectechProtocol : TkStarProtocol
{
    public override short Port => 7019;
}