using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.XeElectech;

[Service(typeof(IProtocol))]
public class XeElectechProtocol : TkStarProtocol
{
    public override int Port => 7019;
}