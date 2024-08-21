using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.CarTrackGPS;

[Service(typeof(IProtocol))]
public class CarTrackGPSProtocol : BaseProtocol
{
    public override int Port => 7033;
    public override byte[] MessageStart => [0x24, 0x24];
}