using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.iTracGPS;

[Service(typeof(IProtocol))]
// ReSharper disable once InconsistentNaming
public class iTracGPSProtocol : TkStarProtocol
{
    public override int Port => 7028;
}