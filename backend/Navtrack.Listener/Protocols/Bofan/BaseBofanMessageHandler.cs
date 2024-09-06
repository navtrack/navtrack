using System.Text.RegularExpressions;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Mappers;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Bofan;

public class BaseBofanMessageHandler<T> : BaseMessageHandler<T>
{
    public override DeviceMessageDocument Parse(MessageInput input)
    {
        GPRMC gprmc = GPRMC.Parse(input.DataMessage.String);

        if (gprmc != null)
        {
            Match deviceIdMatch = new Regex(",(\\d+),(\\d{6}.\\d{3}),").Match(input.DataMessage.String);

            if (deviceIdMatch.Success)
            {
                input.ConnectionContext.SetDevice(deviceIdMatch.Groups[1].Value);
                    
                DeviceMessageDocument deviceMessageDocument = new()
                {
                    // Device = input.ConnectionContext.Device,
                    Position = PositionElementMapper.Map(gprmc)
                };

                return deviceMessageDocument;
            }
        }

        return null;
    }
}