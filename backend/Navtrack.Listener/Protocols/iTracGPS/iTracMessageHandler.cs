using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.iTracGPS;

[Service(typeof(ICustomMessageHandler<iTracGPSProtocol>))]
// ReSharper disable once InconsistentNaming
public class iTracMessageHandler : BaseTkStarMessageHandler<iTracGPSProtocol>;