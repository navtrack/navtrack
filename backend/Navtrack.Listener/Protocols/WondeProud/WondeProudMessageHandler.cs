using System.Text.RegularExpressions;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.WondeProud;

[Service(typeof(ICustomMessageHandler<WondeProudProtocol>))]
public class WondeProudMessageHandler : BaseMessageHandler<WondeProudProtocol>
{
    public override Position Parse(MessageInput input)
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
            input.ConnectionContext.SetDevice(locationMatch.Groups[1].Value);
                
            Position position = new()
            {
                Device = input.ConnectionContext.Device,
                Date = NewDateTimeUtil.Convert(DateFormat.YYYYMMDDHHMMSS, locationMatch.Groups[2].Value),
                Longitude = locationMatch.Groups[3].Get<double>(),
                Latitude = locationMatch.Groups[4].Get<double>(),
                Speed = locationMatch.Groups[5].Get<float?>(),
                Heading = locationMatch.Groups[6].Get<float?>(),
                Altitude = locationMatch.Groups[7].Get<float?>(),
                Satellites = locationMatch.Groups[8].Get<short?>(),
            };

            return position;
        }

        return null;
    }
}