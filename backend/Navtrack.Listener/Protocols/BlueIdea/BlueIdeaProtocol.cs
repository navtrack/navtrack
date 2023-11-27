using Navtrack.Listener.Protocols.Bofan;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.BlueIdea;

[Service(typeof(IProtocol))]
public class BlueIdeaProtocol : BofanProtocol
{
    public override int Port => 7044;
}