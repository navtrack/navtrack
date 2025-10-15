using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.GPSMarker;

[Service(typeof(IProtocol), ServiceLifetime.Singleton)]
public class GPSMarkerProtocol : BaseProtocol
{
    public override short Port => 7047;
    public override byte[] MessageStart => [0x24];
    public override IEnumerable<byte[]> MessageEnd => new[] {new byte[] {0x23}};
}