using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Ruptela;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class RuptelaProtocol : BaseProtocol
{
    public override short Port => 7056;
}