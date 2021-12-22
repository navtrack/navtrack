using System;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Suntech;

[Service(typeof(ICustomMessageHandler<SuntechProtocol>))]
public class SuntechMessageHandler : BaseMessageHandler<SuntechProtocol>
{
    public override Location Parse(MessageInput input)
    {
        input.Client.SetDevice(input.DataMessage.Split.Get<string>(1));
            
        Location location = new()
        {
            Device = input.Client.Device,
            DateTime = ConvertDate(input.DataMessage.Split.Get<string>(3),
                input.DataMessage.Split.Get<string>(4)),
            Latitude = input.DataMessage.Split.Get<double>(6),
            Longitude = input.DataMessage.Split.Get<double>(7),
            Speed = input.DataMessage.Split.Get<float?>(8),
            Heading = input.DataMessage.Split.Get<float?>(9),
            Satellites = input.DataMessage.Split.Get<short?>(10),
            PositionStatus = input.DataMessage.Split.Get<string>(11) == "1",
            Odometer = input.DataMessage.Split.Get<double?>(12)
        };

        return location;
    }

    private static DateTime ConvertDate(string date, string time)
    {
        return DateTimeUtil.New(date[..4], date[4..6], date[6..8],
            time[..2], time[3..5], time[6..8], add2000Year: false);
    }
}