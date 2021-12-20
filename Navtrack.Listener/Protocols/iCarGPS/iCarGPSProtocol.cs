using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.iCarGPS;

[Service(typeof(IProtocol))]
// ReSharper disable once InconsistentNaming
public class iCarGPSProtocol : TkStarProtocol
{
    public override int Port => 7027;
}