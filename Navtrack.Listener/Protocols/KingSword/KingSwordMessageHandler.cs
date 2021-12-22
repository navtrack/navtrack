using System.Globalization;
using System.Text.RegularExpressions;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.KingSword;

[Service(typeof(ICustomMessageHandler<KingSwordProtocol>))]
public class KingSwordMessageHandler : BaseMessageHandler<KingSwordProtocol>
{
    public override Location Parse(MessageInput input)
    {
        Match locationMatch =
            new Regex("(\\d{15})," + // imei
                      "(..)," + // command
                      "(A|V)," + // position status
                      "(.{2})(.{2})(.{2})," + // yy mm dd
                      "(.{2})(.{2})(.{2})," + // hh mm ss
                      "(.)(.{7})," + // latitude
                      "(.)(.{7})," + // longitude
                      "(.{4})," + // speed
                      "(.{4})," + // heading
                      "(.{8})," + // status 
                      "(..)," + // gsm signal
                      "(\\d+)," +// power
                      "(.*?)," + // oil
                      "(.*?)(,|#)" + // odometer
                      "(\\d*)") // altitude
                .Match(input.DataMessage.String);

        if (locationMatch.Success)
        {
            input.Client.SetDevice(locationMatch.Groups[1].Value);
                
            Location location = new()
            {
                Device = input.Client.Device,
                DateTime = DateTimeUtil.NewFromHex(
                    locationMatch.Groups[4].Value,
                    locationMatch.Groups[5].Value,
                    locationMatch.Groups[6].Value,
                    locationMatch.Groups[4].Value,
                    locationMatch.Groups[5].Value,
                    locationMatch.Groups[6].Value,
                    locationMatch.Groups[7].Value),
                Latitude = GetCoordinate(locationMatch.Groups[10].Value, locationMatch.Groups[11].Value),
                Longitude = GetCoordinate(locationMatch.Groups[12].Value, locationMatch.Groups[13].Value),
                Speed = int.Parse(locationMatch.Groups[14].Value, NumberStyles.HexNumber) / 100,
                Heading = locationMatch.Groups[15].Get<float?>(),
                GsmSignal = short.Parse(locationMatch.Groups[17].Value, NumberStyles.HexNumber),
                Odometer = long.Parse(locationMatch.Groups[20].Value, NumberStyles.HexNumber),
                Altitude = locationMatch.Groups[22].Get<float?>(),
            };

            return location;
        }

        return null;
    }

    private static double GetCoordinate(string minus, string p1)
    {
        double coordinate = int.Parse(p1, NumberStyles.HexNumber) / (double)600000;

        return minus == "8" ? -coordinate : coordinate;
    }
}