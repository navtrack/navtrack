using System;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.TkStar;

public class BaseTkStarMessageHandler<T> : BaseMessageHandler<T>
{
    public override DeviceMessageDocument Parse(MessageInput input)
    {
        int timeIndex = input.DataMessage.CommaSplit[2] == "V1" || input.DataMessage.CommaSplit[2] == "V2" ? 3 : 5;

        input.ConnectionContext.SetDevice(input.DataMessage.CommaSplit.Get<string>(1));

        DeviceMessageDocument deviceMessageDocument = new()
        {
            Position = new PositionElement
            {
                Date = ConvertDate(input.DataMessage.CommaSplit.Get<string>(timeIndex),
                    input.DataMessage.CommaSplit.Get<string>(timeIndex + 8)),
                Valid = input.DataMessage.CommaSplit.Get<string>(timeIndex + 1) == "A",
                Latitude = GpsUtil.ConvertDmmLatToDecimal(input.DataMessage.CommaSplit[timeIndex + 2],
                    input.DataMessage.CommaSplit[timeIndex + 3]),
                Longitude = GpsUtil.ConvertDmmLongToDecimal(input.DataMessage.CommaSplit[timeIndex + 4],
                    input.DataMessage.CommaSplit[timeIndex + 5]),
                Speed = SpeedUtil.KnotsToKph(input.DataMessage.CommaSplit.Get<float>(timeIndex + 6)),
                Heading = input.DataMessage.CommaSplit.Get<float?>(timeIndex + 7)
            }
        };

        return deviceMessageDocument;
    }

    private static DateTime ConvertDate(string time, string date) =>
        DateTimeUtil.New(date[4..6], date[2..4], date[..2], time[..2], time[2..4], time[4..6]);
}