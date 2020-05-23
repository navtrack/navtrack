using System.Globalization;
using System.Text.RegularExpressions;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.CarTrackGPS
{
    [Service(typeof(ICustomMessageHandler<CarTrackGPSProtocol>))]
    public class CarTrackGPSMessageHandler : BaseMessageHandler<CarTrackGPSProtocol>
    {
        public override Location Parse(MessageInput input)
        {
            Match locationMatch =
                new Regex("\\$\\$" + // header
                          "(\\d+)(.*?)" + // device id
                          "&A(\\d{4})" + // command
                          "&B" + // gps data header
                          "(\\d{2})(\\d{2})(\\d{2}).(\\d{3})," + // hh mm ss . sss
                          "(A|V)," + // position status
                          "(\\d{4}.\\d{4}),(N|S)," + // latitude
                          "(\\d{5}.\\d{4}),(E|W)," + // longitude
                          "(.*?)," + // speed
                          "(.*?)," + // heading
                          "(\\d{2})(\\d{2})(\\d{2})," + // dd mm yy
                          "(.*?)" +
                          "\\*(..)\\|" + // checksum
                          "(.*)\\|" + // hdop
                          "&C(\\d+)" + // io
                          "&D(.*?)" + // odometer
                          "&E(\\d+)" + // alarm 
                          "(&Y(\\d+)|)" + // ad 
                          "(#*)") // end
                    .Match(input.DataMessage.String);

            if (locationMatch.Success)
            {
                Location location = new Location
                {
                    Device = new Device
                    {
                        DeviceId = locationMatch.Groups[1].Value
                    },
                    DateTime = DateTimeUtil.New(locationMatch.Groups[17].Value, 
                        locationMatch.Groups[16].Value,
                        locationMatch.Groups[15].Value, 
                        locationMatch.Groups[4].Value, 
                        locationMatch.Groups[5].Value,
                        locationMatch.Groups[6].Value, 
                        locationMatch.Groups[7].Value),
                    PositionStatus = locationMatch.Groups[8].Value == "A",
                    Latitude = GpsUtil.ConvertDmmLatToDecimal(locationMatch.Groups[9].Value, locationMatch.Groups[10].Value),
                    Longitude = GpsUtil.ConvertDmmLatToDecimal(locationMatch.Groups[11].Value, locationMatch.Groups[12].Value),
                    Speed = SpeedUtil.KnotsToKph(locationMatch.Groups[13].Get<decimal>()),
                    Heading = locationMatch.Groups[14].Get<decimal?>() / 10,
                    HDOP = locationMatch.Groups[20].Get<decimal?>() / 10,
                    Odometer = GetOdometer(locationMatch.Groups[22].Value)
                };
           
                return location;
            }

            return null;
        }

        private static double? GetOdometer(string value)
        {
            value = value.Replace(':', 'A')
                .Replace(';', 'B')
                .Replace('<', 'C')
                .Replace('=', 'D')
                .Replace('>', 'E')
                .Replace('?', 'F');

            return long.Parse(value, NumberStyles.HexNumber);
        }
    }
}