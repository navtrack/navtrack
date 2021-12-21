using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.Gosafe;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Skypatrol;

[Service(typeof(ICustomMessageHandler<SkypatrolProtocol>))]
public class SkypatrolMessageHandler : BaseGosafeMessageHandler<SkypatrolProtocol>
{
}