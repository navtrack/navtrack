using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.CarTrackGPS;

[Service(typeof(IProtocol))]
public class CarTrackGPSProtocol : BaseProtocol
{
    public override int Port => 7033;
    public override byte[] MessageStart => new byte[] {0x24, 0x24};
}