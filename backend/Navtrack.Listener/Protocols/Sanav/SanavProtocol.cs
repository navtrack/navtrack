using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Sanav;

[Service(typeof(IProtocol))]
public class SanavProtocol : BaseProtocol
{
    public override int Port => 7036;
}