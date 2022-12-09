using System.Globalization;
using System.Text.RegularExpressions;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.GPSMarker;

[Service(typeof(ICustomMessageHandler<GPSMarkerProtocol>))]
public class GPSMarkerMessageHandler : BaseMessageHandler<GPSMarkerProtocol>
{
    public override Location Parse(MessageInput input)
    {
        Match locationMatch =
            new Regex(
                    "\\$GM(\\d)(.{2})?" +
                    "(\\d{15})" + // imei
                    "T(\\d{2}\\d{2}\\d{2}\\d{2}\\d{2}\\d{2})" + // dd mm yy hh mm ss
                    "(N|S)(\\d{2}\\d{2}\\d{4})" + // latitude dd mm ssss
                    "(E|W)(\\d{3}\\d{2}\\d{4})" + // longitude ddd mm ssss
                    "(\\d{3})" + // speed
                    "(\\d{3})" + // heading
                    "(.)" + // satellites hex
                    "(.{2})(\\d)(\\d)(\\d{3})#")
                .Match(input.DataMessage.String);

        if (locationMatch.Success)
        {
            input.Client.SetDevice(locationMatch.Groups[3].Value);
                
            Location location = new()
            {
                Device = input.Client.Device,
                DateTime = NewDateTimeUtil.Convert(DateFormat.DDMMYYHHMMSS, locationMatch.Groups[4].Value),
                Latitude = NewGpsUtil.Convert(GpsFormat.DDDMMmmmm, locationMatch.Groups[6].Value, locationMatch.Groups[5].Value),
                Longitude = NewGpsUtil.Convert(GpsFormat.DDDMMmmmm, locationMatch.Groups[8].Value, locationMatch.Groups[7].Value),
                Speed = locationMatch.Groups[9].Get<float?>(),
                Heading = locationMatch.Groups[10].Get<float?>(),
                Satellites = short.Parse(locationMatch.Groups[11].Value, NumberStyles.HexNumber)
            };

            return location;
        }

        return null;
    }
}