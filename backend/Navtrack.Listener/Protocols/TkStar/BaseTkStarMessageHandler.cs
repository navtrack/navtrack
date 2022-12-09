using System;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.TkStar;

public class BaseTkStarMessageHandler<T> : BaseMessageHandler<T>
{
    public override Location Parse(MessageInput input)
    {
        int timeIndex = input.DataMessage.CommaSplit[2] == "V1" || input.DataMessage.CommaSplit[2] == "V2" ? 3 : 5;

        input.Client.SetDevice(input.DataMessage.CommaSplit.Get<string>(1));

        Location location = new()
        {
            Device = input.Client.Device,
            DateTime = ConvertDate(input.DataMessage.CommaSplit.Get<string>(timeIndex),
                input.DataMessage.CommaSplit.Get<string>(timeIndex + 8)),
            PositionStatus = input.DataMessage.CommaSplit.Get<string>(timeIndex + 1) == "A",
            Latitude = GpsUtil.ConvertDmmLatToDecimal(input.DataMessage.CommaSplit[timeIndex + 2],
                input.DataMessage.CommaSplit[timeIndex + 3]),
            Longitude = GpsUtil.ConvertDmmLongToDecimal(input.DataMessage.CommaSplit[timeIndex + 4],
                input.DataMessage.CommaSplit[timeIndex + 5]),
            Speed = SpeedUtil.KnotsToKph(input.DataMessage.CommaSplit.Get<float>(timeIndex + 6)),
            Heading = input.DataMessage.CommaSplit.Get<float?>(timeIndex + 7)
        };

        return location;
    }

    private static DateTime ConvertDate(string time, string date) =>
        DateTimeUtil.New(date[4..6], date[2..4], date[..2], time[..2], time[2..4], time[4..6]);
}