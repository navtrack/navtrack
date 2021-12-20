using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Galileosky;

[Service(typeof(IProtocol))]
public class GalileoskyProtocol : BaseProtocol
{
    public override int Port => 7055;
}