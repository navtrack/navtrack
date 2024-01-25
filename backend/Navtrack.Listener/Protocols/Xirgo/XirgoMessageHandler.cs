using System;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;
using static System.String;

namespace Navtrack.Listener.Protocols.Xirgo;

[Service(typeof(ICustomMessageHandler<XirgoProtocol>))]
public class XirgoMessageHandler : BaseMessageHandler<XirgoProtocol>
{
    public override Position Parse(MessageInput input)
    {
        input.ConnectionContext.SetDevice(input.DataMessage.CommaSplit[0].Replace("$", Empty));

        Position position = new()
        {
            Device = input.ConnectionContext.Device,
            Date = DateTime.Parse($"{input.DataMessage.CommaSplit[2]} {input.DataMessage.CommaSplit[3]}"),
            Latitude = input.DataMessage.CommaSplit.Get<double>(4),
            Longitude = input.DataMessage.CommaSplit.Get<double>(5),
            Altitude = input.DataMessage.CommaSplit.Get<float?>(6),
            Speed = input.DataMessage.CommaSplit.Get<float?>(7),
        };


        if (input.DataMessage.CommaSplit.Length >= 24)
        {
            position.Heading = input.DataMessage.CommaSplit.Get<float?>(11);
            position.Satellites = input.DataMessage.CommaSplit.Get<short?>(12);
            position.HDOP = input.DataMessage.CommaSplit.Get<float?>(13);
            position.Odometer = input.DataMessage.CommaSplit.Get<double?>(14);
        }
        else
        {
            position.Heading = input.DataMessage.CommaSplit.Get<float?>(8);
            position.Satellites = input.DataMessage.CommaSplit.Get<short?>(9);
            position.HDOP = input.DataMessage.CommaSplit.Get<float?>(10);
            position.Odometer = input.DataMessage.CommaSplit.Get<double?>(13);
        }

        return position;
    }
}