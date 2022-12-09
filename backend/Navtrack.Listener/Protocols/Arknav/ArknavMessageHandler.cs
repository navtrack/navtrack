using System.Text.RegularExpressions;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Arknav;

[Service(typeof(ICustomMessageHandler<ArknavProtocol>))]
public class ArknavMessageHandler : BaseMessageHandler<ArknavProtocol>
{
    public override Location Parse(MessageInput input)
    {
        Match locationMatch =
            new Regex("(\\d{15})," +  // imei
                      "(.{6})," + // id code
                      "(\\d{3})," + // status
                      "(L\\d{3})," + // version
                      "(A|V)," + // position status
                      "(\\d+.\\d+),(N|S)," + // latitude dd mm.mmmm N/S
                      "(\\d+.\\d+),(E|W)," + // longitude ddd mm.mmmm E/W
                      "(.*?)," + // speed
                      "(.*?)," + // heading
                      "(.*?)," + // hdop
                      "(\\d{2}):(\\d{2}):(\\d{2}) (\\d{2})-(\\d{2})-(\\d{2}),") // hh:mm:ss dd-mm-yy
                .Match(input.DataMessage.String);

        if (locationMatch.Success)
        {
            input.Client.SetDevice(locationMatch.Groups[1].Value);
                
            Location location = new()
            {
                Device = input.Client.Device,
                PositionStatus = locationMatch.Groups[5].Value == "A",
                Latitude = GpsUtil.ConvertDmmLatToDecimal(locationMatch.Groups[6].Value,
                    locationMatch.Groups[7].Value),
                Longitude = GpsUtil.ConvertDmmLatToDecimal(locationMatch.Groups[8].Value,
                    locationMatch.Groups[9].Value),
                Speed = SpeedUtil.KnotsToKph(locationMatch.Groups[10].Get<float>()),
                Heading = locationMatch.Groups[11].Get<float?>(),
                HDOP = locationMatch.Groups[12].Get<float?>(),
                DateTime = DateTimeUtil.New(locationMatch.Groups[18].Value, locationMatch.Groups[17].Value,
                    locationMatch.Groups[16].Value, locationMatch.Groups[13].Value, locationMatch.Groups[14].Value,
                    locationMatch.Groups[15].Value)
            };

            return location;
        }

        return null;
    }
}