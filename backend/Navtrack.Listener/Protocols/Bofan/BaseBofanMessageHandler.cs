using System.Text.RegularExpressions;
using Navtrack.Database.Model.Devices;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Listener.Services.Mappers;

namespace Navtrack.Listener.Protocols.Bofan;

public class BaseBofanMessageHandler<T> : BaseMessageHandler<T>
{
    public override DeviceMessageEntity? Parse(MessageInput input)
    {
        GPRMC? gprmc = GPRMC.Parse(input.DataMessage.String);

        if (gprmc != null)
        {
            Match deviceIdMatch = new Regex(",(\\d+),(\\d{6}.\\d{3}),").Match(input.DataMessage.String);

            if (deviceIdMatch.Success)
            {
                input.ConnectionContext.SetDevice(deviceIdMatch.Groups[1].Value);

                DeviceMessageEntity deviceMessage = new();

                DeviceMessageEntityMapper.Map(gprmc, deviceMessage);
                
                return deviceMessage;
            }
        }

        return null;
    }
}