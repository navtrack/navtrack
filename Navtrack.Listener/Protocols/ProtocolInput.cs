using System.Net.Sockets;
using System.Threading;

namespace Navtrack.Listener.Protocols
{
    public class ProtocolInput
    {
        public NetworkStream NetworkStream { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
}