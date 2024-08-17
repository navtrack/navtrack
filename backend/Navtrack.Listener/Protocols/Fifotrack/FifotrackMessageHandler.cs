using System;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Fifotrack;

[Service(typeof(ICustomMessageHandler<FifotrackProtocol>))]
public class FifotrackMessageHandler : BaseMessageHandler<FifotrackProtocol>
{
    public override DeviceMessageDocument Parse(MessageInput input)
    {
        input.ConnectionContext.SetDevice(input.DataMessage.CommaSplit[1]);

        DeviceMessageDocument deviceMessageDocument = new()
        {
            // Device = input.ConnectionContext.Device,
            Position = new PositionElement
            {
                Date = ConvertDate(input.DataMessage.CommaSplit.Get<string>(5)),
                Valid = input.DataMessage.CommaSplit.Get<string>(6) == "A",
                Latitude = input.DataMessage.CommaSplit.Get<double>(7),
                Longitude = input.DataMessage.CommaSplit.Get<double>(8),
                Speed = input.DataMessage.CommaSplit.Get<float?>(9),
                Heading = input.DataMessage.CommaSplit.Get<float?>(10),
                Altitude = input.DataMessage.CommaSplit.Get<float?>(11),
                Odometer = input.DataMessage.CommaSplit.Get<double?>(12)
            }
        };

        return deviceMessageDocument;
    }

    private static DateTime ConvertDate(string date) => DateTimeUtil.New(date[..2], date[2..4], date[4..6],
        date[6..8], date[8..10], date[10..12]);
}