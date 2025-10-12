using System.Text.RegularExpressions;
using Navtrack.Database.Model.Devices;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Eview;

[Service(typeof(ICustomMessageHandler<EviewProtocol>))]
public class EviewMessageHandler : BaseMessageHandler<EviewProtocol>
{
    public override DeviceMessageEntity? Parse(MessageInput input)
    {
        DeviceMessageEntity deviceMessageDocument = Parse(input, HandleLogin, HandleLocation);

        return deviceMessageDocument;
    }

    private static DeviceMessageEntity? HandleLogin(MessageInput input)
    {
        Match imeiMatch = new Regex("!1,(\\d{15})").Match(input.DataMessage.String);

        if (imeiMatch.Success)
        {
            input.ConnectionContext.SetDevice(imeiMatch.Groups[1].Value);
        }

        return null;
    }

    private static DeviceMessageEntity? HandleLocation(MessageInput input)
    {
        if (input.ConnectionContext.Device != null)
        {
            string locationRegex = "(\\d+\\/\\d+\\/\\d+)," + // date dd/mm/yy
                                   "(\\d+:\\d+:\\d+)," + // time hh:mm:ss
                                   "(.*?)," + // latitude
                                   "(.*?)," + // longitude
                                   "(.*?)," + // speed
                                   "(.*?)," + // heading
                                   "((.*?)," + // flags
                                   "(.*?)," + //altitude
                                   "(\\d+)," + // battery
                                   "(\\d+)," + // satellites in use
                                   "(\\d+),)?"; // satellites in view

            Match locationMatch =
                new Regex(locationRegex)
                    .Match(input.DataMessage.String);

            if (locationMatch.Success)
            {
                DeviceMessageEntity deviceMessage = new()
                {
                    Date = DateTimeUtil.Convert(DateFormat.DDMMYY_HHMMSS, locationMatch.Groups[1].Value,
                        locationMatch.Groups[2].Value),
                    Latitude = locationMatch.Groups[3].Get<double>(),
                    Longitude = locationMatch.Groups[4].Get<double>(),
                    Speed = locationMatch.Groups[5].Get<short?>(),
                    Heading = locationMatch.Groups[6].Get<short?>(),
                    Altitude = locationMatch.Groups[9].Get<short?>(),
                    Satellites = locationMatch.Groups[11].Get<short?>()
                };

                return deviceMessage;
            }
        }

        return null;
    }
}