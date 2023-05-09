using Navtrack.Library.DI;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Jointech;

[Service(typeof(ICustomMessageHandler<JointechProtocol>))]
public class JointechMessageHandler : BaseMessageHandler<JointechProtocol>
{
    public override Location Parse(MessageInput input)
    {
        Location location = Parse(input,
            JointechV1MessageHandler.Parse,
            JointechV2MessageHandler.Parse);

        return location;
    }
}