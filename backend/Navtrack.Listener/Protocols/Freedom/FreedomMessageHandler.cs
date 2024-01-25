using System;
using System.Text.RegularExpressions;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Freedom;

[Service(typeof(ICustomMessageHandler<FreedomProtocol>))]
public class FreedomMessageHandler : BaseMessageHandler<FreedomProtocol>
{
    public override Position Parse(MessageInput input)
    {
        Match locationMatch =
            new Regex(
                    "IMEI,(\\d{15})," + // imei
                    "(\\d{4}\\/\\d{2}\\/\\d{2}), " + // date YYYY/mm/dd
                    "(\\d{2}:\\d{2}:\\d{2}), " + // hh:mm:ss
                    "(N|S), Lat:(\\d+.\\d+), " + // latitude
                    "(E|W), Lon:(\\d+.\\d+), " + // longitude
                    "Spd:(\\d+.\\d+)")
                .Match(input.DataMessage.String);

        if (locationMatch.Success)
        {
            input.ConnectionContext.SetDevice(locationMatch.Groups[1].Value);

            Position position = new()
            {
                Device = input.ConnectionContext.Device,
                Date = DateTime.Parse($"{locationMatch.Groups[2].Value} {locationMatch.Groups[3].Value}"),
                Latitude = GpsUtil.ConvertDmmLatToDecimal(locationMatch.Groups[5].Value,
                    locationMatch.Groups[4].Value),
                Longitude = GpsUtil.ConvertDmmLatToDecimal(locationMatch.Groups[7].Value,
                    locationMatch.Groups[6].Value),
                Speed = SpeedUtil.KnotsToKph(locationMatch.Groups[8].Get<float>())
            };

            return position;
        }

        return null;
    }
}