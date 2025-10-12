using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Freedom;

[Service(typeof(IProtocol))]
public class FreedomProtocol : BaseProtocol
{
    public override short Port => 7049;
}