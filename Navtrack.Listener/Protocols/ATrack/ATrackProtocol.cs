using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.ATrack;

[Service(typeof(IProtocol))]
public class ATrackProtocol : BaseProtocol
{
    public override int Port => 7061;
}