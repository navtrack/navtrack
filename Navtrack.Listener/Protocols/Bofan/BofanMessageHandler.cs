using System.Text.RegularExpressions;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Bofan
{
    [Service(typeof(ICustomMessageHandler<BofanProtocol>))]
    public class BofanMessageHandler : BaseMessageHandler<BofanProtocol>
    {
        public override Location Parse(MessageInput input)
        {
            Match locationMatch =
                new Regex("\\$(.*?)," +
                          "(\\d+)," + //id
                          "(\\d{2})(\\d{2})(\\d{2}).(\\d{3})," + // time hh mm ss . sss
                          "(A|V)," + // gps fix
                          "(\\d+.\\d+),(N|S)," + // latitude
                          "(\\d+.\\d+),(E|W)," + // longitude
                          "(.*?)," + // speed
                          "(.*?)," + // heading
                          "(\\d{2})(\\d{2})(\\d{2})") // date dd mm yy
                    .Match(input.DataMessage.String);

            if (locationMatch.Groups.Count == 17)
            {
                Location location = new Location
                {
                    Device = new Device
                    {
                        DeviceId = locationMatch.Groups[2].Value
                    },
                    DateTime = DateTimeUtil.New(locationMatch.Groups[16].Value, 
                        locationMatch.Groups[15].Value,
                        locationMatch.Groups[14].Value, 
                        locationMatch.Groups[3].Value, 
                        locationMatch.Groups[4].Value,
                        locationMatch.Groups[5].Value, 
                        locationMatch.Groups[6].Value),
                    PositionStatus = locationMatch.Groups[7].Value == "A",
                    Latitude = GpsUtil.ConvertDmmLatToDecimal(locationMatch.Groups[8].Value, locationMatch.Groups[9].Value),
                    Longitude = GpsUtil.ConvertDmmLatToDecimal(locationMatch.Groups[10].Value, locationMatch.Groups[11].Value),
                    Speed = SpeedUtil.KnotsToKph(locationMatch.Groups[12].Get<decimal>()),
                    Heading = locationMatch.Groups[13].Get<decimal?>()
                };
           
                return location;
            }

            return null;
        }
    }
}