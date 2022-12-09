using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.iTracGPS;

[Service(typeof(IProtocol))]
// ReSharper disable once InconsistentNaming
public class iTracGPSProtocol : TkStarProtocol
{
    public override int Port => 7028;
}