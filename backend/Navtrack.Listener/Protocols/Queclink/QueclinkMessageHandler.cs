using System.Globalization;
using System.Text.RegularExpressions;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Queclink;

[Service(typeof(ICustomMessageHandler<QueclinkProtocol>))]
public class QueclinkMessageHandler : BaseMessageHandler<QueclinkProtocol>
{
    public override DeviceMessageDocument Parse(MessageInput input)
    {
        Match imeiMatch = new Regex(@",(\d{15}),").Match(input.DataMessage.String);

        Match locationMatch =
            new Regex(
                    @"(.*),(.*),(.*),(.*),(.*),(-?.*),(-?.*),(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2}),(.*?),(.*?),(.*?),(.*?),00,")
                .Match(input.DataMessage.String);

        if (imeiMatch.Success && locationMatch.Success)
        {
            input.ConnectionContext.SetDevice(imeiMatch.Groups[1].Value);

            DeviceMessageDocument deviceMessageDocument = new()
            {
                Position = new PositionElement
                {
                    HDOP = locationMatch.Groups[2].Get<float?>(),
                    Speed = locationMatch.Groups[3].Get<float?>(),
                    Heading = locationMatch.Groups[4].Get<float?>(),
                    Altitude = locationMatch.Groups[5].Get<float?>(),
                    Longitude = locationMatch.Groups[6].Get<double>(),
                    Latitude = locationMatch.Groups[7].Get<double>(),
                    Date = DateTimeUtil.New(locationMatch.Groups[8].Value, locationMatch.Groups[9].Value,
                        locationMatch.Groups[10].Value,
                        locationMatch.Groups[11].Value, locationMatch.Groups[12].Value, locationMatch.Groups[13].Value,
                        add2000Year: false),
                },
                Gsm = new GsmElement
                {
                    CellGlobalIdentity = new CellGlobalIdentityElement
                    {
                        MobileCountryCode = locationMatch.Groups[14].Get<int?>().ToString(),
                        MobileNetworkCode = locationMatch.Groups[15].Get<int?>().ToString(),
                        LocationAreaCode = int.Parse(locationMatch.Groups[16].Value, NumberStyles.HexNumber).ToString(),
                        CellId = int.Parse(locationMatch.Groups[17].Value, NumberStyles.HexNumber)
                    }
                }
            };

            return deviceMessageDocument;
        }

        return null;
    }
}