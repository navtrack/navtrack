using System.Text.RegularExpressions;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Gotop
{
    [Service(typeof(ICustomMessageHandler<GotopProtocol>))]
    public class GotopMessageHandler : BaseMessageHandler<GotopProtocol>
    {
        public override Location Parse(MessageInput input)
        {
            Match locationMatch =
                new Regex("(\\d{15})" + // imei
                          ",(.*?)," + // command
                          "(A|V)," + // position status
                          "DATE:(\\d{2})(\\d{2})(\\d{2})," + // yy mm dd
                          "TIME:(\\d{2})(\\d{2})(\\d{2})," + // hh mm ss
                          "LAT:(.*?)(N|S)," + // latitude dd.ddddddd N/S
                          "LOT:(.*?)(E|W)," + // longitude ddd.ddddddd E/W
                          "Speed:(.*?)," + // speed km/h
                          "(.*?)," + //status
                          "(\\d+)") // heading
                    .Match(input.DataMessage.String);

            if (locationMatch.Success)
            {
                Location location = new Location
                {
                    Device = new Device
                    {
                        IMEI = locationMatch.Groups[1].Value
                    },
                    DateTime = DateTimeUtil.New(
                        locationMatch.Groups[4].Value,
                        locationMatch.Groups[5].Value,
                        locationMatch.Groups[6].Value,
                        locationMatch.Groups[7].Value,
                        locationMatch.Groups[8].Value,
                        locationMatch.Groups[9].Value),
                    PositionStatus = locationMatch.Groups[3].Value == "A",
                    Latitude = GetCoordinate(locationMatch.Groups[10].Get<decimal>(), locationMatch.Groups[11].Value),
                    Longitude = GetCoordinate(locationMatch.Groups[12].Get<decimal>(), locationMatch.Groups[13].Value),
                    Speed = locationMatch.Groups[14].Get<decimal?>(),
                    Heading = locationMatch.Groups[16].Get<decimal?>()
                };

                return location;
            }

            return null;
        }

        private static decimal GetCoordinate(decimal coordinate, string value)
        {
            if (value == "S" || value == "W")
            {
                return -coordinate;
            }

            return coordinate;
        }
    }
}