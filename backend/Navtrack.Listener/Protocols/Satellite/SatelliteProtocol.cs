using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Satellite;

[Service(typeof(IProtocol))]
public class SatelliteProtocol : BaseProtocol
{
    public override int Port => 7059;
}