using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.WondeProud;

[Service(typeof(IProtocol))]
public class WondeProudProtocol : BaseProtocol
{
    public override int Port => 7046;
}