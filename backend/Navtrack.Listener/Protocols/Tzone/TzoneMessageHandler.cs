using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Mappers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Tzone;

[Service(typeof(ICustomMessageHandler<TzoneProtocol>))]
public class TzoneMessageHandler : BaseMessageHandler<TzoneProtocol>
{
    public override DeviceMessageDocument Parse(MessageInput input)
    {
        GPRMC gprmc = GPRMC.Parse(input.DataMessage.BarSplit[1].Substring(2));

        input.ConnectionContext.SetDevice(input.DataMessage.BarSplit[0].Substring(4));
            
        DeviceMessageDocument deviceMessageDocument = new()
        {
            // Device = input.ConnectionContext.Device,
            Position = PositionElementMapper.Map(gprmc)
        };

        deviceMessageDocument.Position.HDOP = input.DataMessage.BarSplit.Get<float?>(3);
        deviceMessageDocument.Position.Odometer = input.DataMessage.BarSplit.GetDouble(11, 4);

        return deviceMessageDocument;
    }
}