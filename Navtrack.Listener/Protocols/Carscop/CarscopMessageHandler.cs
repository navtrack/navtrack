using System.Text.RegularExpressions;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Protocols.TkStar;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Carscop;

[Service(typeof(ICustomMessageHandler<CarscopProtocol>))]
public class CarscopMessageHandler : BaseTkStarMessageHandler<CarscopProtocol>
{
    public override Location Parse(MessageInput input)
    {
        if (input.DataMessage.Bytes[^1] == 0x23)
        {
            return base.Parse(input);
        }

        HandleLoginMessage(input);

        Location location = ParseLocation(input);

        return location;
    }

    private static Location ParseLocation(MessageInput input)
    {
        GroupCollection lgc =
            new Regex(
                    @"(\d{2})(\d{2})(\d{2})(A|V)(\d{4}.\d{4})(N|S)(\d{5}.\d{4})(E|W)(.{5})(\d{2})(\d{2})(\d{2})(.{5})(.{8})(L)(\d{6})")
                .Matches(input.DataMessage.String)[0].Groups;

        if (lgc.Count == 17)
        {
            Location location = new()
            {
                Device = input.Client.Device,
                DateTime = DateTimeUtil.New(lgc[10].Value, lgc[11].Value, lgc[12].Value, lgc[1].Value, lgc[2].Value,
                    lgc[3].Value),
                PositionStatus = lgc[2].Value == "A",
                Latitude = GpsUtil.ConvertDmmLatToDecimal(lgc[5].Value, lgc[6].Value),
                Longitude = GpsUtil.ConvertDmmLongToDecimal(lgc[7].Value, lgc[8].Value),
                Speed = SpeedUtil.KnotsToKph(lgc[9].Get<float>()),
                Heading = float.Parse(lgc[13].Value),
                Odometer = double.Parse(lgc[16].Value)
            };

            return location;
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
                input.Client.SetDevice(input.DataMessage.String.Substring(17, 15));
                  
                // ReSharper disable once UnreachableCode
                const string validImei = true ? "1" : "0";

                string reply = $"*{serial}DX06{validImei}^";

                input.NetworkStream.Write(StringUtil.ConvertStringToByteArray(reply));
            }
        }
    }
}