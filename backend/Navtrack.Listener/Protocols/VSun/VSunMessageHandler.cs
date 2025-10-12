using System.Text.RegularExpressions;
using Navtrack.Database.Model.Devices;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Listener.Services.Mappers;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.VSun;

[Service(typeof(ICustomMessageHandler<VSunProtocol>))]
public class VSunMessageHandler : BaseMessageHandler<VSunProtocol>
{
    public override DeviceMessageEntity? Parse(MessageInput input)
    {
        DeviceMessageEntity deviceMessage = Parse(input, HandleImei, HandleLocation);

        return deviceMessage;
    }

    private static DeviceMessageEntity? HandleLocation(MessageInput input)
    {
        GPRMC? gprmc = GPRMC.Parse(input.DataMessage.String);

        if (gprmc != null)
        {
            DeviceMessageEntity deviceMessage = new();
            
            DeviceMessageEntityMapper.Map(gprmc, deviceMessage);

            return deviceMessage;
        }
            
        return null;
    }

    private static DeviceMessageEntity? HandleImei(MessageInput input)
    {
        if (input.ConnectionContext.Device == null)
        {
            Match locationMatch =
                new Regex("#(\\d{15})#" + // imei
                          "(.*?)#") // device model
                    .Match(input.DataMessage.String);

            if (locationMatch.Success)
            {
                input.ConnectionContext.SetDevice(locationMatch.Groups[1].Value);
            }
        }

        return null;
    }
}