using System;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;
using static System.String;

namespace Navtrack.Listener.Protocols.Xirgo;

[Service(typeof(ICustomMessageHandler<XirgoProtocol>))]
public class XirgoMessageHandler : BaseMessageHandler<XirgoProtocol>
{
    public override DeviceMessageDocument Parse(MessageInput input)
    {
        input.ConnectionContext.SetDevice(input.DataMessage.CommaSplit[0].Replace("$", Empty));

        DeviceMessageDocument deviceMessageDocument = new()
        {
            Position = new PositionElement
            {
                Date = DateTime.Parse($"{input.DataMessage.CommaSplit[2]} {input.DataMessage.CommaSplit[3]}"),
                Latitude = input.DataMessage.CommaSplit.Get<double>(4),
                Longitude = input.DataMessage.CommaSplit.Get<double>(5),
                Altitude = input.DataMessage.CommaSplit.Get<float?>(6),
                Speed = input.DataMessage.CommaSplit.Get<float?>(7)
            }
        };


        if (input.DataMessage.CommaSplit.Length >= 24)
        {
            deviceMessageDocument.Position.Heading = input.DataMessage.CommaSplit.Get<float?>(11);
            deviceMessageDocument.Position.Satellites = input.DataMessage.CommaSplit.Get<short?>(12);
            deviceMessageDocument.Position.HDOP = input.DataMessage.CommaSplit.Get<float?>(13);
            deviceMessageDocument.Device ??= new DeviceElement();
            deviceMessageDocument.Device.Odometer = input.DataMessage.CommaSplit.Get<uint?>(14);
        }
        else
        {
            deviceMessageDocument.Position.Heading = input.DataMessage.CommaSplit.Get<float?>(8);
            deviceMessageDocument.Position.Satellites = input.DataMessage.CommaSplit.Get<short?>(9);
            deviceMessageDocument.Position.HDOP = input.DataMessage.CommaSplit.Get<float?>(10);
            deviceMessageDocument.Device ??= new DeviceElement();
            deviceMessageDocument.Device.Odometer = input.DataMessage.CommaSplit.Get<uint?>(13);
        }

        return deviceMessageDocument;
    }
}