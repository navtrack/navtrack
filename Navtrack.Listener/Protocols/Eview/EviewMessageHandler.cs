using System.Text.RegularExpressions;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Eview;

[Service(typeof(ICustomMessageHandler<EviewProtocol>))]
public class EviewMessageHandler : BaseMessageHandler<EviewProtocol>
{
    public override Location Parse(MessageInput input)
    {
        Location location = Parse(input, HandleLogin, HandleLocation);

        return location;
    }

    private static Location HandleLogin(MessageInput input)
    {
        Match imeiMatch = new Regex("!1,(\\d{15})").Match(input.DataMessage.String);

        if (imeiMatch.Success)
        {
            input.Client.SetDevice(imeiMatch.Groups[1].Value);
        }

        return null;
    }

    private static Location HandleLocation(MessageInput input)
    {
        if (input.Client.Device != null)
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
                Location location = new()
                {
                    Device = input.Client.Device,
                    DateTime = NewDateTimeUtil.Convert(DateFormat.DDMMYY_HHMMSS, locationMatch.Groups[1].Value,
                        locationMatch.Groups[2].Value),
                    Latitude = locationMatch.Groups[3].Get<double>(),
                    Longitude = locationMatch.Groups[4].Get<double>(),
                    Speed = locationMatch.Groups[5].Get<float?>(),
                    Heading = locationMatch.Groups[6].Get<float?>(),
                    Altitude = locationMatch.Groups[9].Get<float?>(),
                    Satellites = locationMatch.Groups[11].Get<short?>()
                };

                return location;
            }
        }

        return null;
    }
}