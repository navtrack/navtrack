using System;
using Navtrack.Database.Model.Devices;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Fifotrack;

[Service(typeof(ICustomMessageHandler<FifotrackProtocol>))]
public class FifotrackMessageHandler : BaseMessageHandler<FifotrackProtocol>
{
    public override DeviceMessageEntity? Parse(MessageInput input)
    {
        input.ConnectionContext.SetDevice(input.DataMessage.CommaSplit[1]);

        DeviceMessageEntity deviceMessage = new()
        {
            Date = ConvertDate(input.DataMessage.CommaSplit.Get<string>(5)),
            Valid = input.DataMessage.CommaSplit.Get<string>(6) == "A",
            Latitude = input.DataMessage.CommaSplit.Get<double>(7),
            Longitude = input.DataMessage.CommaSplit.Get<double>(8),
            Speed = input.DataMessage.CommaSplit.Get<short?>(9),
            Heading = input.DataMessage.CommaSplit.Get<short?>(10),
            Altitude = input.DataMessage.CommaSplit.Get<short?>(11),
            DeviceOdometer = input.DataMessage.CommaSplit.Get<int?>(12)
        };

        return deviceMessage;
    }

    private static DateTime ConvertDate(string date) => DateTimeUtil.New(date[..2], date[2..4], date[4..6],
        date[6..8], date[8..10], date[10..12]);
}