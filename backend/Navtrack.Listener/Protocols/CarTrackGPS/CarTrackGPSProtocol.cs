using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.CarTrackGPS;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class CarTrackGPSProtocol : BaseProtocol
{
    public override short Port => 7033;
    public override byte[] MessageStart => [0x24, 0x24];
}