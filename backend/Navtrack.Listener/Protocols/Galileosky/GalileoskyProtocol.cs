using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Galileosky;

[Service(typeof(IProtocol))]
public class GalileoskyProtocol : BaseProtocol
{
    public override int Port => 7055;
}