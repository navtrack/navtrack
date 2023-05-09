using System.Globalization;
using System.Text.RegularExpressions;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Jointech;

public class JointechV1MessageHandler
{
    public static Location Parse(MessageInput input)
    {
        Match locationMatch =
            new Regex("24" + // header
                      "(\\d{10})" + // terminal id
                      "(.)" + // protocol version
                      "(.)" + // data type
                      "(.{4})" + // data length
                      "(\\d{2})(\\d{2})(\\d{2})" + // dd mm yy
                      "(\\d{2})(\\d{2})(\\d{2})" + // hh mm ss
                      "(\\d{8})" + // latitude dd mm . mmmm
                      "(\\d{9})" + // longitude ddd mm . mmmm
                      "(.)" + // locating indication
                      "(.{2})" + // speed
                      "(.{2})" + // direction
                      "(.{2})" + // fuel level high 8 bit
                      "(.{8})" + // status
                      "(.{8})" + // mileage
                      "(.{2})" + // fuel level low 8 bit
                      "(.{2})") // message serial number
                .Match(input.DataMessage.HexString);

        if (locationMatch.Success)
        {
            Location location = new()
            {
                DateTime = DateTimeUtil.New(
                    locationMatch.Groups[7].Value,
                    locationMatch.Groups[6].Value,
                    locationMatch.Groups[5].Value,
                    locationMatch.Groups[8].Value,
                    locationMatch.Groups[9].Value,
                    locationMatch.Groups[10].Value),
                Latitude = GetCoordinate(locationMatch.Groups[11].Value, locationMatch.Groups[13].Value[0], 1,
                    "(\\d{4})(\\d{4})"),
                Longitude = GetCoordinate(locationMatch.Groups[12].Value, locationMatch.Groups[13].Value[0], 2,
                    "(\\d{5})(\\d{4})"),
                Speed = SpeedUtil.KnotsToKph(int.Parse(locationMatch.Groups[14].Value, NumberStyles.HexNumber)),
                Heading = int.Parse(locationMatch.Groups[15].Value, NumberStyles.HexNumber),
                Odometer = long.Parse(locationMatch.Groups[18].Value, NumberStyles.HexNumber),
                PositionStatus = BitUtil.IsTrue(locationMatch.Groups[13].Value[0], 0)
            };

            input.Client.SetDevice(locationMatch.Groups[1].Value);
            location.Device = input.Client.Device;

            return location;
        }

        return null;
    }

    private static double GetCoordinate(string value, char locatingIndicator, int index, string regex)
    {
        Match locationMatch =
            new Regex("(\\d{4})(\\d{4})")
                .Match(value);

        string direction = BitUtil.IsTrue(locatingIndicator, 1) ? "N" : "S";

        return GpsUtil.ConvertDmmLatToDecimal($"{locationMatch.Groups[1].Value}.{locationMatch.Groups[2].Value}",
            direction);
    }
}