using System.Collections.Generic;
using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.SinoTrack;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Carscop
{
    [Service(typeof(IProtocol))]
    public class CarscopProtocol : SinoTrackProtocol
    {
        public override int Port => 7016;
        public override IEnumerable<byte[]> MessageEnd => new List<byte[]> {new byte[] {0x23}, new byte[] {0x5E}};
    }
}