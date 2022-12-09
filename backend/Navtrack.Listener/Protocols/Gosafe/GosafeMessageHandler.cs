using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Gosafe;

[Service(typeof(ICustomMessageHandler<GosafeProtocol>))]
public class GosafeMessageHandler : BaseGosafeMessageHandler<GosafeProtocol>
{
}