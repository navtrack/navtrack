using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Satellite;

[Service(typeof(IProtocol))]
public class SatelliteProtocol : BaseProtocol
{
    public override short Port => 7059;
}