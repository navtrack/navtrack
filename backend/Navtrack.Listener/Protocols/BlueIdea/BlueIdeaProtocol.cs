using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.Bofan;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.BlueIdea;

[Service(typeof(IProtocol))]
public class BlueIdeaProtocol : BofanProtocol
{
    public override int Port => 7044;
}