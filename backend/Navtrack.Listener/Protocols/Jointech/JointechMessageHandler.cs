using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Jointech;

[Service(typeof(ICustomMessageHandler<JointechProtocol>))]
public class JointechMessageHandler : BaseMessageHandler<JointechProtocol>
{
    public override Position Parse(MessageInput input)
    {
        Position position = Parse(input,
            JointechV1MessageHandler.Parse,
            JointechV2MessageHandler.Parse);

        return position;
    }
}