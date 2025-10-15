using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Navtelecom;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class NavtelecomProtocol : BaseProtocol
{
    public override short Port => 7054;
}