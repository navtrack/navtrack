using System.Text.RegularExpressions;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.GoPass;

[Service(typeof(ICustomMessageHandler<GoPassProtocol>))]
public class GoPassMessageHandler : BaseMessageHandler<GoPassProtocol>
{
    public override Position Parse(MessageInput input)
    {
        HandleImeiMessage(input);

        Position position = ParseLocation(input);

        return position;
    }

    private static void HandleImeiMessage(MessageInput input)
    {
        if (input.ConnectionContext.Device == null)
        {
            Match imeiMatch = new Regex("(\\d{15})").Match(input.DataMessage.String);

            if (imeiMatch.Success)
            {
                input.ConnectionContext.SetDevice(imeiMatch.Groups[1].Value);
            }
        }
    }

    private static Position ParseLocation(MessageInput input)
    {
        if (input.ConnectionContext.Device == null)
        {
            return null;
        }
            
        Match locationMatch =
            new Regex(
                    "GPRMC," +
                    "(\\d{2})(\\d{2})(\\d{2})(.|)(\\d+|)," + // hh mm ss . sss
                    "(A|V)," + // gps fix
                    "(\\d+.\\d+),(N|S)," + // latitude
                    "(\\d+.\\d+),(W|E)," + // longitude
                    "(\\d+.\\d+)," + // speed
                    "(\\d+.\\d+)," + // heading
                    "(\\d{2})(\\d{2})(\\d{2}),") // dd mm yy
                .Match(input.DataMessage.String);

        if (locationMatch.Success)
        {
            Position position = new()
            {
                Device = input.ConnectionContext.Device,
                Date = DateTimeUtil.New(
                    locationMatch.Groups[15].Value,
                    locationMatch.Groups[14].Value,
                    locationMatch.Groups[13].Value,
                    locationMatch.Groups[1].Value,
                    locationMatch.Groups[2].Value,
                    locationMatch.Groups[3].Value,
                    locationMatch.Groups[5].Value),
                PositionStatus = locationMatch.Groups[6].Value == "A",
                Latitude = GpsUtil.ConvertDmmLatToDecimal(locationMatch.Groups[7].Value, locationMatch.Groups[8].Value),
                Longitude = GpsUtil.ConvertDmmLongToDecimal(locationMatch.Groups[9].Value, locationMatch.Groups[10].Value),
                Speed = SpeedUtil.KnotsToKph(locationMatch.Groups[11].Get<float>()),
                Heading = locationMatch.Groups[12].Get<float?>()
            };

            return position;
        }

        return null;
    }
}