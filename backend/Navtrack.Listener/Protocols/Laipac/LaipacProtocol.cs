using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Laipac;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class LaipacProtocol : BaseProtocol
{
    public override short Port => 7052;
}