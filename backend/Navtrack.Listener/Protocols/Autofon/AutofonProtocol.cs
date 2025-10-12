using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Autofon;

[Service(typeof(IProtocol))]
public class AutofonProtocol : BaseProtocol
{
    public override short Port => 7060;
}