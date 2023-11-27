using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.StarLink;

[Service(typeof(IProtocol))]
public class ErmProtocol : BaseProtocol
{
    public override int Port => 7051;
}