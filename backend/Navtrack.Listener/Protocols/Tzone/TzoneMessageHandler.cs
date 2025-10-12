using Navtrack.Database.Model.Devices;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Listener.Services.Mappers;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Tzone;

[Service(typeof(ICustomMessageHandler<TzoneProtocol>))]
public class TzoneMessageHandler : BaseMessageHandler<TzoneProtocol>
{
    public override DeviceMessageEntity? Parse(MessageInput input)
    {
        GPRMC? gprmc = GPRMC.Parse(input.DataMessage.BarSplit[1].Substring(2));

        input.ConnectionContext.SetDevice(input.DataMessage.BarSplit[0].Substring(4));

        DeviceMessageEntity deviceMessage = new();
        DeviceMessageEntityMapper.Map(gprmc, deviceMessage);

        deviceMessage.HDOP = input.DataMessage.BarSplit.Get<float?>(3);
        deviceMessage.DeviceOdometer = input.DataMessage.BarSplit.Get<int>(11);

        return deviceMessage;
    }
}