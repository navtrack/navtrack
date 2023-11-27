using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.iCarGPS;

[Service(typeof(ICustomMessageHandler<iCarGPSProtocol>))]
// ReSharper disable once InconsistentNaming
public class iCarGPSMessageHandler : BaseTkStarMessageHandler<iCarGPSProtocol>
{
}