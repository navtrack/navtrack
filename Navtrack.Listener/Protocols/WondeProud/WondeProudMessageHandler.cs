using System.Text.RegularExpressions;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.WondeProud
{
    [Service(typeof(ICustomMessageHandler<WondeProudProtocol>))]
    public class WondeProudMessageHandler : BaseMessageHandler<WondeProudProtocol>
    {
        public override Location Parse(MessageInput input)
        {
            Match locationMatch =
                new Regex(
                        "(\\d+)," + // device id
                        "(\\d{4}\\d{2}\\d{2}\\d{2}\\d{2}\\d{2})," + // YYYY mm dd hh mm ss
                        "(-?\\d+.\\d+)," + // longitude
                        "(-?\\d+.\\d+)," + // latitude
                        "(\\d+)," + // speed
                        "(\\d+)," + // heading
                        "(.*?)," + // altitude
                        "(\\d+)") // satellites
                    .Match(input.DataMessage.String);

            if (locationMatch.Success)
            {
                Location location = new Location
                {
                    Device = new Device
                    {
                        IMEI = locationMatch.Groups[1].Value
                    },
                    DateTime = NewDateTimeUtil.Convert(DateFormat.YYYYMMDDHHMMSS, locationMatch.Groups[2].Value),
                    Longitude = locationMatch.Groups[3].Get<decimal>(),
                    Latitude = locationMatch.Groups[4].Get<decimal>(),
                    Speed = locationMatch.Groups[5].Get<decimal?>(),
                    Heading = locationMatch.Groups[6].Get<decimal?>(),
                    Altitude = locationMatch.Groups[7].Get<decimal?>(),
                    Satellites = locationMatch.Groups[8].Get<short?>(),
                };

                return location;
            }

            return null;
        }
    }
}