using System.Text.RegularExpressions;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Topfly;

[Service(typeof(ICustomMessageHandler<TopflyProtocol>))]
public class TopflyMessageHandler : BaseMessageHandler<TopflyProtocol>
{
    public override Location Parse(MessageInput input)
    {
        Match locationMatch =
            new Regex("\\((\\d+)" + // imei
                      "(.*?)(\\d{12})" + // date time yy mm dd hh mm ss
                      "(A|V)" + // gps fix
                      "(\\d+.\\d+)(N|S)" + // latitude
                      "(\\d+.\\d+)(E|W)" + // longitude
                      "(\\d{3}.\\d)" + // speed
                      "(\\d+.\\d+)") // heading
                .Match(input.DataMessage.String);

        if (locationMatch.Success)
        {
            input.Client.SetDevice(locationMatch.Groups[1].Value);

            Location location = new()
            {
                Device = input.Client.Device,
                DateTime = NewDateTimeUtil.Convert(DateFormat.YYMMDDHHMMSS, locationMatch.Groups[3].Value),
                PositionStatus = locationMatch.Groups[4].Value == "A",
                Latitude = GpsUtil.ConvertDmmLatToDecimal(locationMatch.Groups[5].Value,
                    locationMatch.Groups[6].Value),
                Longitude = GpsUtil.ConvertDmmLongToDecimal(locationMatch.Groups[7].Value,
                    locationMatch.Groups[8].Value),
                Speed = locationMatch.Groups[9].Get<float?>(),
                Heading = locationMatch.Groups[10].Get<float?>()
            };

            return location;
        }

        return null;
    }
}