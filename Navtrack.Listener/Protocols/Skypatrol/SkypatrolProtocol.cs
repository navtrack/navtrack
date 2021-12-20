using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.Gosafe;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Skypatrol;

[Service(typeof(IProtocol))]
public class SkypatrolProtocol : GosafeProtocol
{
    public override int Port => 7023;
}