using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.iTracGPS;

[Service(typeof(ICustomMessageHandler<iTracGPSProtocol>))]
// ReSharper disable once InconsistentNaming
public class iTracMessageHandler : BaseTkStarMessageHandler<iTracGPSProtocol>
{
}