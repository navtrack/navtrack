using System.Globalization;
using System.Text.RegularExpressions;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.KeSon;

[Service(typeof(ICustomMessageHandler<KeSonProtocol>))]
public class KeSonMessageHandler : BaseMessageHandler<KeSonProtocol>
{
    public override Location Parse(MessageInput input)
    {
        Match locationMatch =
            new Regex("#(\\d+)#" + //imei
                      "(.*?)#" + // username
                      "(.*?)#" + // acc status
                      "(.*?)#" + // extend byte
                      "(.*?)#" + // data type
                      "(.*?)#" + // extend byte
                      "(.*?)#" + // lbs
                      "(\\d+.\\d+),(E|W)," + // longitude
                      "(\\d+.\\d+),(N|S)," + // latitude
                      "(.*)?," + // speed
                      "(\\d+)#" + // heading
                      "(\\d{2})(\\d{2})(\\d{2})#" + // date dd mm yy
                      "(\\d{2})(\\d{2})(\\d{2})#") // time hh mm ss
                .Match(input.DataMessage.String);

        if (locationMatch.Success)
        {
            input.Client.SetDevice(locationMatch.Groups[1].Value);
                
            Location location = new()
            {
                Device = input.Client.Device,
                DateTime = DateTimeUtil.New(
                    locationMatch.Groups[16].Value,
                    locationMatch.Groups[15].Value,
                    locationMatch.Groups[14].Value,
                    locationMatch.Groups[17].Value,
                    locationMatch.Groups[18].Value,
                    locationMatch.Groups[19].Value),
                Latitude = GpsUtil.ConvertDmmLatToDecimal(locationMatch.Groups[10].Value,
                    locationMatch.Groups[11].Value),
                Longitude = GpsUtil.ConvertDmmLongToDecimal(locationMatch.Groups[8].Value,
                    locationMatch.Groups[9].Value),
                Speed = SpeedUtil.KnotsToKph(locationMatch.Groups[12].Get<float>()),
                Heading = locationMatch.Groups[15].Get<float>(),
                Odometer = long.Parse(locationMatch.Groups[18].Value, NumberStyles.HexNumber),
                PositionStatus = locationMatch.Groups[7].Value != "V"
            };

            return location;
        }

        return null;
    }
}