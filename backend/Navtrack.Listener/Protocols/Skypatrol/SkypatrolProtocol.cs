using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Protocols.Gosafe;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Skypatrol;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class SkypatrolProtocol : GosafeProtocol
{
    public override short Port => 7023;
}