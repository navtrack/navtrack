using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Autofon;

[Service(typeof(IProtocol))]
public class AutofonProtocol : BaseProtocol
{
    public override int Port => 7060;
}