using System;
using Navtrack.Database.Model.Devices;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;
using static System.String;

namespace Navtrack.Listener.Protocols.Xirgo;

[Service(typeof(ICustomMessageHandler<XirgoProtocol>))]
public class XirgoMessageHandler : BaseMessageHandler<XirgoProtocol>
{
    public override DeviceMessageEntity? Parse(MessageInput input)
    {
        input.ConnectionContext.SetDevice(input.DataMessage.CommaSplit[0].Replace("$", Empty));

        DeviceMessageEntity deviceMessage = new()
        {
            Date = DateTime.Parse($"{input.DataMessage.CommaSplit[2]} {input.DataMessage.CommaSplit[3]}"),
            Latitude = input.DataMessage.CommaSplit.Get<double>(4),
            Longitude = input.DataMessage.CommaSplit.Get<double>(5),
            Altitude = input.DataMessage.CommaSplit.Get<short?>(6),
            Speed = input.DataMessage.CommaSplit.Get<short?>(7)
        };


        if (input.DataMessage.CommaSplit.Length >= 24)
        {
            deviceMessage.Heading = input.DataMessage.CommaSplit.Get<short?>(11);
            deviceMessage.Satellites = input.DataMessage.CommaSplit.Get<short?>(12);
            deviceMessage.HDOP = input.DataMessage.CommaSplit.Get<float?>(13);
            deviceMessage.DeviceOdometer = input.DataMessage.CommaSplit.Get<int?>(14);
        }
        else
        {
            deviceMessage.Heading = input.DataMessage.CommaSplit.Get<short?>(8);
            deviceMessage.Satellites = input.DataMessage.CommaSplit.Get<short?>(9);
            deviceMessage.HDOP = input.DataMessage.CommaSplit.Get<float?>(10);
            deviceMessage.DeviceOdometer = input.DataMessage.CommaSplit.Get<int?>(13);
        }

        return deviceMessage;
    }
}