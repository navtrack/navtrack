using System;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using static System.String;

namespace Navtrack.Listener.Protocols.Xirgo;

[Service(typeof(ICustomMessageHandler<XirgoProtocol>))]
public class XirgoMessageHandler : BaseMessageHandler<XirgoProtocol>
{
    public override Location Parse(MessageInput input)
    {
        input.Client.SetDevice(input.DataMessage.CommaSplit[0].Replace("$", Empty));

        Location location = new()
        {
            Device = input.Client.Device,
            DateTime = DateTime.Parse($"{input.DataMessage.CommaSplit[2]} {input.DataMessage.CommaSplit[3]}"),
            Latitude = input.DataMessage.CommaSplit.Get<double>(4),
            Longitude = input.DataMessage.CommaSplit.Get<double>(5),
            Altitude = input.DataMessage.CommaSplit.Get<float?>(6),
            Speed = input.DataMessage.CommaSplit.Get<float?>(7),
        };


        if (input.DataMessage.CommaSplit.Length >= 24)
        {
            location.Heading = input.DataMessage.CommaSplit.Get<float?>(11);
            location.Satellites = input.DataMessage.CommaSplit.Get<short?>(12);
            location.HDOP = input.DataMessage.CommaSplit.Get<float?>(13);
            location.Odometer = input.DataMessage.CommaSplit.Get<double?>(14);
        }
        else
        {
            location.Heading = input.DataMessage.CommaSplit.Get<float?>(8);
            location.Satellites = input.DataMessage.CommaSplit.Get<short?>(9);
            location.HDOP = input.DataMessage.CommaSplit.Get<float?>(10);
            location.Odometer = input.DataMessage.CommaSplit.Get<double?>(13);
        }

        return location;
    }
}