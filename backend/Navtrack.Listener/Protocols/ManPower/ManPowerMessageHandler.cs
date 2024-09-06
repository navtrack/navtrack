using System.Text.RegularExpressions;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.ManPower;

[Service(typeof(ICustomMessageHandler<ManPowerProtocol>))]
public class ManPowerMessageHandler : BaseMessageHandler<ManPowerProtocol>
{
    public override DeviceMessageDocument Parse(MessageInput input)
    {
        Match locationMatch =
            new Regex(
                    "simei:(\\d+)," + // imei
                    "(.*?)" + // ignore
                    "(\\d{12})," + // yy mm dd hh mm ss
                    "(A|V)," + // gps fix
                    "(\\d+.\\d+),(N|S)," + // latitude
                    "(\\d+.\\d+),(E|W)," + // longitude
                    "(\\d+.\\d+),") // speed
                .Match(input.DataMessage.String);

        if (locationMatch.Success)
        {
            input.ConnectionContext.SetDevice(locationMatch.Groups[1].Value);

            DeviceMessageDocument deviceMessageDocument = new()
            {
                Position = new PositionElement
                {
                    Date = NewDateTimeUtil.Convert(DateFormat.YYMMDDHHMMSS, locationMatch.Groups[3].Value),
                    Valid = locationMatch.Groups[4].Value == "A",
                    Latitude = GpsUtil.ConvertDmmLatToDecimal(locationMatch.Groups[5].Value,
                        locationMatch.Groups[6].Value),
                    Longitude = GpsUtil.ConvertDmmLongToDecimal(locationMatch.Groups[7].Value,
                        locationMatch.Groups[8].Value),
                    Speed = locationMatch.Groups[9].Get<float?>()
                }
            };

            return deviceMessageDocument;
        }

        return null;
    }
}