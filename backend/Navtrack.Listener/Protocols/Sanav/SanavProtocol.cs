using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Sanav;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class SanavProtocol : BaseProtocol
{
    public override short Port => 7036;
}