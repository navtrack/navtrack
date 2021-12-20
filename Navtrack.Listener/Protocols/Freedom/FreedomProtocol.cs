using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Freedom;

[Service(typeof(IProtocol))]
public class FreedomProtocol : BaseProtocol
{
    public override int Port => 7049;
}