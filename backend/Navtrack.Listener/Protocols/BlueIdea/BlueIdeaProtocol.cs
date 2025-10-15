using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Protocols.Bofan;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.BlueIdea;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class BlueIdeaProtocol : BofanProtocol
{
    public override short Port => 7044;
}