using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.StarLink;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class ErmProtocol : BaseProtocol
{
    public override short Port => 7051;
}