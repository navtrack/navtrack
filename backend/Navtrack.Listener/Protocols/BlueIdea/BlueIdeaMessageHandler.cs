using Navtrack.Library.DI;
using Navtrack.Listener.Protocols.Bofan;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.BlueIdea;

[Service(typeof(ICustomMessageHandler<BlueIdeaProtocol>))]
public class BlueIdeaMessageHandler : BaseBofanMessageHandler<BlueIdeaProtocol>
{
}