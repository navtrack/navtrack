using System.Globalization;
using System.Text.RegularExpressions;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Pretrace;

[Service(typeof(ICustomMessageHandler<PretraceProtocol>))]
public class PretraceMessageHandler : BaseMessageHandler<PretraceProtocol>
{
    public override Location Parse(MessageInput input)
    {
        Match locationMatch =
            new Regex("(\\()" +
                      "(\\d{15})" + // IMEI
                      "(U\\d\\d\\d)" + // Type
                      "(\\d)" + // GPS Type
                      "(A|V)" + // Position status
                      "(\\d{2})(\\d{2})(\\d{2})(\\d{2})(\\d{2})(\\d{2})" + // Date yy mm dd hh mm ss
                      "(\\d{4}.\\d{4})(N|S)" + // Latitude dd mm.mmmm N/S
                      "(\\d{5}.\\d{4})(E|W)" + // Longitude ddd mm.mmmm E/W
                      "(\\d{3})" + // Speed
                      "(\\d{3})" + // Heading
                      "(.{3})" + // Altitude HEX
                      "(.{8})" + // Odometer HEX
                      "(.)" + // Satellites HEX
                      "(\\d{2})" + // HDOP
                      "(\\d{2})" + // GSM signal
                      "(.*)" + // ???
                      "\\^(..)") // Checksum
                .Match(input.DataMessage.String);

        if (locationMatch.Success)
        {
            input.Client.SetDevice(locationMatch.Groups[2].Value);

            Location location = new()
            {
                Device = input.Client.Device,
                PositionStatus = locationMatch.Groups[5].Value == "A",
                DateTime = DateTimeUtil.New(locationMatch.Groups[6].Value, locationMatch.Groups[7].Value,
                    locationMatch.Groups[8].Value, locationMatch.Groups[9].Value, locationMatch.Groups[10].Value,
                    locationMatch.Groups[11].Value),
                Latitude = GpsUtil.ConvertDmmLatToDecimal(locationMatch.Groups[12].Value,
                    locationMatch.Groups[13].Value),
                Longitude = GpsUtil.ConvertDmmLatToDecimal(locationMatch.Groups[14].Value,
                    locationMatch.Groups[15].Value),
                Speed = locationMatch.Groups[16].Get<float?>(),
                Heading = locationMatch.Groups[17].Get<float?>(),
                Altitude = int.Parse(locationMatch.Groups[18].Value, NumberStyles.HexNumber),
                Odometer = int.Parse(locationMatch.Groups[19].Value, NumberStyles.HexNumber),
                Satellites = short.Parse(locationMatch.Groups[20].Value, NumberStyles.HexNumber),
                HDOP = locationMatch.Groups[21].Get<float?>(),
                GsmSignal = locationMatch.Groups[22].Get<short?>()
            };

            return location;
        }

        return null;
    }
}