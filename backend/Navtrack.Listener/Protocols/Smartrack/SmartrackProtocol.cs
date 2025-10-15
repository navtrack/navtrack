using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Protocols.VjoyCar;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Smartrack;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class SmartrackProtocol : VjoyCarProtocol
{
    public override short Port => 7025;
}