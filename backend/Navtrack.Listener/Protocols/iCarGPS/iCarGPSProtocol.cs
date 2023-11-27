using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.iCarGPS;

[Service(typeof(IProtocol))]
// ReSharper disable once InconsistentNaming
public class iCarGPSProtocol : TkStarProtocol
{
    public override int Port => 7027;
}