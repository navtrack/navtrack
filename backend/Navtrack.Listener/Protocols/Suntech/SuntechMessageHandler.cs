using System;
using Navtrack.Database.Model.Devices;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Suntech;

[Service(typeof(ICustomMessageHandler<SuntechProtocol>))]
public class SuntechMessageHandler : BaseMessageHandler<SuntechProtocol>
{
    public override DeviceMessageEntity? Parse(MessageInput input)
    {
        input.ConnectionContext.SetDevice(input.DataMessage.Split.Get<string>(1));

        DeviceMessageEntity deviceMessage = new()
        {
            Date = ConvertDate(input.DataMessage.Split.Get<string>(3),
                input.DataMessage.Split.Get<string>(4)),
            Latitude = input.DataMessage.Split.Get<double>(6),
            Longitude = input.DataMessage.Split.Get<double>(7),
            Speed = input.DataMessage.Split.Get<float?>(8).ToShort(),
            Heading = input.DataMessage.Split.Get<float?>(9).ToShort(),
            Satellites = input.DataMessage.Split.Get<short?>(10),
            Valid = input.DataMessage.Split.Get<string>(11) == "1",
            DeviceOdometer = input.DataMessage.Split.Get<int?>(12)
        };

        return deviceMessage;
    }

    private static DateTime ConvertDate(string date, string time)
    {
        return DateTimeUtil.New(date[..4], date[4..6], date[6..8],
            time[..2], time[3..5], time[6..8], add2000Year: false);
    }
}