using System.Globalization;
using System.Text.RegularExpressions;
using Navtrack.Database.Model.Devices;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Carscop;

[Service(typeof(ICustomMessageHandler<CarscopProtocol>))]
public class CarscopMessageHandler : BaseTkStarMessageHandler<CarscopProtocol>
{
    public override DeviceMessageEntity? Parse(MessageInput input)
    {
        if (input.DataMessage.Bytes[^1] == 0x23)
        {
            return base.Parse(input);
        }

        HandleLoginMessage(input);

        DeviceMessageEntity? deviceMessage = ParseLocation(input);

        return deviceMessage;
    }

    private static DeviceMessageEntity? ParseLocation(MessageInput input)
    {
        GroupCollection lgc =
            new Regex(
                    @"(\d{2})(\d{2})(\d{2})(A|V)(\d{4}.\d{4})(N|S)(\d{5}.\d{4})(E|W)(.{5})(\d{2})(\d{2})(\d{2})(.{5})(.{8})(L)(\d{6})")
                .Matches(input.DataMessage.String)[0].Groups;

        if (lgc.Count == 17)
        {
            DeviceMessageEntity deviceMessage = new()
            {
                Date = DateTimeUtil.New(lgc[10].Value, lgc[11].Value, lgc[12].Value, lgc[1].Value, lgc[2].Value,
                    lgc[3].Value),
                Valid = lgc[2].Value == "A",
                Latitude = GpsUtil.ConvertDmmLatToDecimal(lgc[5].Value, lgc[6].Value),
                Longitude = GpsUtil.ConvertDmmLongToDecimal(lgc[7].Value, lgc[8].Value),
                Speed = SpeedUtil.KnotsToKph(lgc[9].Get<float>()),
                Heading = (short)double.Parse(lgc[13].Value, CultureInfo.InvariantCulture),
                DeviceOdometer = int.Parse(lgc[16].Value)
            };

            return deviceMessage;
        }

        return null;
    }

    private static void HandleLoginMessage(MessageInput input)
    {
        if (input.DataMessage.Bytes[^1] == 0x5E)
        {
            string serial = input.DataMessage.String.Substring(1, 12);
            string command = input.DataMessage.String.Substring(13, 4);

            if (command == "UB05")
            {
                input.ConnectionContext.SetDevice(input.DataMessage.String.Substring(17, 15));

                // ReSharper disable once UnreachableCode
                const string validImei = true ? "1" : "0";

                string reply = $"*{serial}DX06{validImei}^";

                input.NetworkStream.Write(StringUtil.ConvertStringToByteArray(reply));
            }
        }
    }
}