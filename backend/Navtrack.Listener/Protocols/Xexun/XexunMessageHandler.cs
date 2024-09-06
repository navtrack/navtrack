using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Xexun;

[Service(typeof(ICustomMessageHandler<XexunProtocol>))]
public class XexunMessageHandler : BaseMessageHandler<XexunProtocol>
{
    public override DeviceMessageDocument Parse(MessageInput input)
    {
        // TODO: join patterns
        GroupCollection lgc =
            new Regex(
                    @"GPRMC,(\d{2})(\d{2})(\d{2}).(\d{3}),(A|V),(\d{4}.\d{4}),(N|S),(\d{5}.\d{4}),(E|W),(.*?),(.*?),(\d{2})(\d{2})(\d{2})(.*?\*..,)(\w)(.*?imei:)(\d{15})")
                .Matches(input.DataMessage.String)[0].Groups;

        if (lgc.Count == 19)
        {
            input.ConnectionContext.SetDevice(lgc[18].Value);

            DeviceMessageDocument deviceMessageDocument = new()
            {
                Position = new PositionElement
                {
                    Date = DateTimeUtil.New(lgc[12].Value, lgc[13].Value, lgc[14].Value, lgc[1].Value, lgc[2].Value,
                        lgc[3].Value, lgc[4].Value),
                    Valid = lgc[16].Value == "F",
                    Latitude = GpsUtil.ConvertDmmLatToDecimal(lgc[6].Value, lgc[7].Value),
                    Longitude = GpsUtil.ConvertDmmLongToDecimal(lgc[8].Value, lgc[9].Value),
                    Speed = SpeedUtil.KnotsToKph(lgc[10].Get<float>()),
                    Heading = string.IsNullOrEmpty(lgc[11].Value) ? default(float?) : float.Parse(lgc[11].Value)
                }
            };

            MatchCollection extra =
                new Regex(@"(.*?imei:)(\d{15}),(\d+),(.*?),(F:)(.*?)V,(1|0),(.*?),(.*?),(\d+),(\d+),(.*?),(\d+)")
                    .Matches(input.DataMessage.String);

            if (extra.Any())
            {
                GroupCollection match = extra[0].Groups;

                if (match.Count == 14)
                {
                    deviceMessageDocument.Position.Satellites = short.Parse(match[3].Value);
                    deviceMessageDocument.Position.Altitude = float.Parse(match[4].Value);
                    deviceMessageDocument.Gsm = new GsmElement
                    {
                        CellGlobalIdentity = new CellGlobalIdentityElement
                        {
                            MobileCountryCode = match[10].Value,
                            MobileNetworkCode = int.Parse(match[11].Value, NumberStyles.HexNumber).ToString(),
                            LocationAreaCode = int.Parse(match[12].Value, NumberStyles.HexNumber).ToString(),
                            CellId = int.Parse(match[13].Value, NumberStyles.HexNumber)
                        }
                    };
                }
            }

            return deviceMessageDocument;
        }

        return null;
    }
}