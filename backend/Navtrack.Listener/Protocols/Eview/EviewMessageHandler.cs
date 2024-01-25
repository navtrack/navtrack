using System.Text.RegularExpressions;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Eview;

[Service(typeof(ICustomMessageHandler<EviewProtocol>))]
public class EviewMessageHandler : BaseMessageHandler<EviewProtocol>
{
    public override Position Parse(MessageInput input)
    {
        Position position = Parse(input, HandleLogin, HandleLocation);

        return position;
    }

    private static Position HandleLogin(MessageInput input)
    {
        Match imeiMatch = new Regex("!1,(\\d{15})").Match(input.DataMessage.String);

        if (imeiMatch.Success)
        {
            input.ConnectionContext.SetDevice(imeiMatch.Groups[1].Value);
        }

        return null;
    }

    private static Position HandleLocation(MessageInput input)
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
                Position position = new()
                {
                    Device = input.ConnectionContext.Device,
                    Date = NewDateTimeUtil.Convert(DateFormat.DDMMYY_HHMMSS, locationMatch.Groups[1].Value,
                        locationMatch.Groups[2].Value),
                    Latitude = locationMatch.Groups[3].Get<double>(),
                    Longitude = locationMatch.Groups[4].Get<double>(),
                    Speed = locationMatch.Groups[5].Get<float?>(),
                    Heading = locationMatch.Groups[6].Get<float?>(),
                    Altitude = locationMatch.Groups[9].Get<float?>(),
                    Satellites = locationMatch.Groups[11].Get<short?>()
                };

                return position;
            }
        }

        return null;
    }
}