using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.iCarGPS;

[Service(typeof(ICustomMessageHandler<iCarGPSProtocol>))]
// ReSharper disable once InconsistentNaming
public class iCarGPSMessageHandler : BaseTkStarMessageHandler<iCarGPSProtocol>
{
}