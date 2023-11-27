using Navtrack.Listener.Protocols.Gosafe;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Skypatrol;

[Service(typeof(ICustomMessageHandler<SkypatrolProtocol>))]
public class SkypatrolMessageHandler : BaseGosafeMessageHandler<SkypatrolProtocol>
{
}