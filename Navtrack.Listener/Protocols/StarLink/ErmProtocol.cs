using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.StarLink;

[Service(typeof(IProtocol))]
public class ErmProtocol : BaseProtocol
{
    public override int Port => 7051;
}