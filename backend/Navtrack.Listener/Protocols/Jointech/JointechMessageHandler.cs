using Navtrack.Database.Model.Devices;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Jointech;

[Service(typeof(ICustomMessageHandler<JointechProtocol>))]
public class JointechMessageHandler : BaseMessageHandler<JointechProtocol>
{
    public override DeviceMessageEntity? Parse(MessageInput input)
    {
        DeviceMessageEntity deviceMessage = Parse(input,
            JointechV1MessageHandler.Parse,
            JointechV2MessageHandler.Parse);

        return deviceMessage;
    }
}