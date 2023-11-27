using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.ATrack;

[Service(typeof(IProtocol))]
public class ATrackProtocol : BaseProtocol
{
    public override int Port => 7061;
}