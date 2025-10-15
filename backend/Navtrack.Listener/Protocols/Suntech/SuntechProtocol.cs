using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Suntech;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class SuntechProtocol : BaseProtocol
{
    public override short Port => 7010;
    public override string SplitMessageBy => ";";
}