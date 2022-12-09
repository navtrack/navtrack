using System.Text.RegularExpressions;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Sanav;

[Service(typeof(ICustomMessageHandler<SanavProtocol>))]
public class SanavMessageHandler : BaseMessageHandler<SanavProtocol>
{
    public override Location Parse(MessageInput input)
    {
        Match locationMatch =
            new Regex("imei(.)" +
                      "(\\d+)" + // imei
                      "(.*?)GPRMC," +
                      "(\\d{2})(\\d{2})(\\d{2}).(\\d{2,3})," + // time hh mm ss . ss(s)
                      "(A|V)," + // position status
                      "(\\d{2}\\d{2}.\\d+),(N|S)," + // latitude
                      "(\\d{2}\\d{3}.\\d+),(E|W)," + // longitude
                      "(\\d+.\\d+)," + // speed
                      "(\\d+.\\d+|)," + // heading
                      "(\\d{2})(\\d{2})(\\d{2})") // date dd mm yy
                .Match(input.DataMessage.String);

        if (locationMatch.Success)
        {
            input.Client.SetDevice(locationMatch.Groups[2].Value);
                
            Location location = new()
            {
                Device = input.Client.Device,
                DateTime = DateTimeUtil.New(
                    locationMatch.Groups[17].Value,
                    locationMatch.Groups[16].Value,
                    locationMatch.Groups[15].Value,
                    locationMatch.Groups[4].Value,
                    locationMatch.Groups[5].Value,
                    locationMatch.Groups[6].Value,
                    locationMatch.Groups[7].Value),
                PositionStatus = locationMatch.Groups[8].Value == "A",
                Latitude = GpsUtil.ConvertDmmLatToDecimal(locationMatch.Groups[9].Value, locationMatch.Groups[10].Value),
                Longitude = GpsUtil.ConvertDmmLongToDecimal(locationMatch.Groups[11].Value, locationMatch.Groups[12].Value),
                Speed = SpeedUtil.KnotsToKph(locationMatch.Groups[14].Get<float>()),
                Heading = locationMatch.Groups[15].Get<float?>()
            };

            return location;
        }

        return null;
    }
}