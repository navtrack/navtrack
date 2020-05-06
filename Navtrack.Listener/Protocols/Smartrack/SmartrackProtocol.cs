using System.Collections.Generic;
using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.SinoTrack;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Smartrack
{
    [Service(typeof(IProtocol))]
    public class SmartrackProtocol : SinoTrackProtocol
    {
        public override int Port => 7020;
        public override byte[] MessageStart => new byte[] { 0x28 };
        public override IEnumerable<byte[]> MessageEnd => new List<byte[]> {new byte[] {0x29}};
    }
}