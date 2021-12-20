using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.ManPower;

[Service(typeof(IProtocol))]
public class ManPowerProtocol : BaseProtocol
{
    public override int Port => 7045;
}