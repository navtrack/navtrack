using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Neomatica;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class NeomaticaProtocol : BaseProtocol
{
    public override short Port => 7058;
}