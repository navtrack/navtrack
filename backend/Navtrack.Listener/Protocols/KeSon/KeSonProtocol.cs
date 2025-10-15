using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.KeSon;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class KeSonProtocol : BaseProtocol
{
    public override short Port => 7041;
}