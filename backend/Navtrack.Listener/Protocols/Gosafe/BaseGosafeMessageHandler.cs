using System.Text.RegularExpressions;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Gosafe;

public class BaseGosafeMessageHandler<T> : BaseMessageHandler<T>
{
    public override Location Parse(MessageInput input)
    {
        if (input.Client.Device == null)
        {
            Match imeiMatch = new Regex(@"(\*GS\d{2}),(\d{15})").Match(input.DataMessage.String);

            if (imeiMatch.Success)
            {
                input.Client.SetDevice(imeiMatch.Groups[2].Value);
            }
        }

        if (input.Client.Device != null)
        {
            Match dateMatch =
                new Regex(@"(,|\$|^)(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2}),").Match(input.DataMessage.String);

            Match locationMatch =
                new Regex(@"GPS:(.);(.*?);(N|S)(.*?);(E|W)(.*?);(.*?);(.*?)(;(.*?);(.*?)(\$|#|$|,|;)|,)")
                    .Match(input.DataMessage.String);

            if (dateMatch.Success && locationMatch.Success && locationMatch.Groups.Count >= 9)
            {
                Location location = new()
                {
                    Device = input.Client.Device,
                    DateTime = DateTimeUtil.New(dateMatch.Groups[7].Value, dateMatch.Groups[6].Value,
                        dateMatch.Groups[5].Value, dateMatch.Groups[2].Value, dateMatch.Groups[3].Value,
                        dateMatch.Groups[4].Value),
                    PositionStatus = locationMatch.Groups[1].Value == "A",
                    Satellites = locationMatch.Groups[2].Get<short>(),
                    Latitude = GpsUtil.ConvertStringToDecimal(locationMatch.Groups[4].Value,
                        locationMatch.Groups[3].Value),
                    Longitude = GpsUtil.ConvertStringToDecimal(locationMatch.Groups[6].Value,
                        locationMatch.Groups[5].Value),
                    Speed = locationMatch.Groups[7].Get<float?>(),
                    Heading = locationMatch.Groups[8].Get<float?>(),
                    Altitude = locationMatch.Groups.Count == 13 ? locationMatch.Groups[10].Get<float?>() : null,
                    HDOP = locationMatch.Groups.Count == 13 ? locationMatch.Groups[11].Get<float?>() : null
                };

                return location;
            }
        }

        return null;
    }
}