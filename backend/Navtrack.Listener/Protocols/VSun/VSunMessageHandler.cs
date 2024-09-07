using System.Text.RegularExpressions;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Listener.Services.Mappers;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.VSun;

[Service(typeof(ICustomMessageHandler<VSunProtocol>))]
public class VSunMessageHandler : BaseMessageHandler<VSunProtocol>
{
    public override DeviceMessageDocument Parse(MessageInput input)
    {
        DeviceMessageDocument deviceMessageDocument = Parse(input, HandleImei, HandleLocation);

        return deviceMessageDocument;
    }

    private static DeviceMessageDocument HandleLocation(MessageInput input)
    {
        GPRMC gprmc = GPRMC.Parse(input.DataMessage.String);

        if (gprmc != null)
        {
            DeviceMessageDocument deviceMessageDocument = new()
            {
                // Device = input.ConnectionContext.Device
                Position = PositionElementMapper.Map(gprmc)
            };

            return deviceMessageDocument;
        }
            
        return null;
    }

    private static DeviceMessageDocument HandleImei(MessageInput input)
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