using Navtrack.Listener.Protocols.Bofan;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.BlueIdea;

[Service(typeof(ICustomMessageHandler<BlueIdeaProtocol>))]
public class BlueIdeaMessageHandler : BaseBofanMessageHandler<BlueIdeaProtocol>;