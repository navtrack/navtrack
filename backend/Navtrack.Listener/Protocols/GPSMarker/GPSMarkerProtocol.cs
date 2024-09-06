using System.Collections.Generic;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.GPSMarker;

[Service(typeof(IProtocol))]
public class GPSMarkerProtocol : BaseProtocol
{
    public override int Port => 7047;
    public override byte[] MessageStart => [0x24];
    public override IEnumerable<byte[]> MessageEnd => new[] {new byte[] {0x23}};
}