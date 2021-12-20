using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.WondeProud;

[Service(typeof(IProtocol))]
public class WondeProudProtocol : BaseProtocol
{
    public override int Port => 7046;
}