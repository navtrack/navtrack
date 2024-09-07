using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Listener.Services.Mappers;
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
            Position = PositionElementMapper.Map(gprmc)
        };

        deviceMessageDocument.Position.HDOP = input.DataMessage.BarSplit.Get<float?>(3);
        deviceMessageDocument.Device ??= new DeviceElement();
        deviceMessageDocument.Device.Odometer = input.DataMessage.BarSplit.Get<int>(11);

        return deviceMessageDocument;
    }
}