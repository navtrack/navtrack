using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Jointech;

[Service(typeof(ICustomMessageHandler<JointechProtocol>))]
public class JointechMessageHandler : BaseMessageHandler<JointechProtocol>
{
    public override DeviceMessageDocument Parse(MessageInput input)
    {
        DeviceMessageDocument deviceMessageDocument = Parse(input,
            JointechV1MessageHandler.Parse,
            JointechV2MessageHandler.Parse);

        return deviceMessageDocument;
    }
}