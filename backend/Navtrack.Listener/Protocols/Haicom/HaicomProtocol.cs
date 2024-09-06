using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Haicom;

[Service(typeof(IProtocol))]
public class HaicomProtocol : BaseProtocol
{
    public override int Port => 7032;
    public override byte[] MessageStart => [0x24];
}