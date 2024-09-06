using System;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Totem;

[Service(typeof(ICustomMessageHandler<TotemProtocol>))]
public class TotemMessageHandler : BaseMessageHandler<TotemProtocol>
{
    public override DeviceMessageDocument Parse(MessageInput input)
    {
        DeviceMessageDocument deviceMessageDocument = Parse(input, ParseLocation_AT07, ParseLocation_AT09);

        return deviceMessageDocument;
    }

    private static DeviceMessageDocument ParseLocation_AT07(MessageInput input)
    {
        input.ConnectionContext.SetDevice(input.DataMessage.Reader.Skip(8).GetUntil('|'));

        DeviceMessageDocument deviceMessageDocument = new DeviceMessageDocument
        {
            Position = new PositionElement(),
            Gsm = new GsmElement()
        };


        deviceMessageDocument.Position.Date = ConvertDate(input.DataMessage.Reader.Skip(8).Get(12));
        deviceMessageDocument.Position.Satellites = Convert.ToInt16(input.DataMessage.Reader.Skip(16).Get(2));
        deviceMessageDocument.Gsm.SignalStrength = GsmUtil.ConvertSignal(Convert.ToInt16(input.DataMessage.Reader.Get(2)));
        deviceMessageDocument.Position.Heading = Convert.ToInt32(input.DataMessage.Reader.Get(3));
        deviceMessageDocument.Position.Speed = Convert.ToInt32(input.DataMessage.Reader.Get(3));
        deviceMessageDocument.Position.HDOP = float.Parse(input.DataMessage.Reader.Get(4));
        deviceMessageDocument.Device ??= new DeviceElement();
        deviceMessageDocument.Device.Odometer = uint.Parse(input.DataMessage.Reader.Get(7));
        deviceMessageDocument.Position.Latitude = GpsUtil.ConvertDmmLatToDecimal(input.DataMessage.Reader.Get(9),
            input.DataMessage.Reader.Get(1));
        deviceMessageDocument.Position.Longitude = GpsUtil.ConvertDmmLongToDecimal(input.DataMessage.Reader.Get(10),
            input.DataMessage.Reader.Get(1));


        return deviceMessageDocument;
    }

    private static DeviceMessageDocument ParseLocation_AT09(MessageInput input)
    {
        input.ConnectionContext.SetDevice(input.DataMessage.Reader.Skip(8).GetUntil('|'));

        DeviceMessageDocument deviceMessageDocument = new DeviceMessageDocument
        {
            Position = new PositionElement(),
            Gsm = new GsmElement()
        };

        deviceMessageDocument.Position.Date = ConvertDate(input.DataMessage.Reader.Skip(8).Get(12));
        deviceMessageDocument.Position.Satellites = Convert.ToInt16(input.DataMessage.Reader.Skip(36).Get(2));
        deviceMessageDocument.Gsm.SignalStrength =
            GsmUtil.ConvertSignal(Convert.ToInt16(input.DataMessage.Reader.Get(2)));
        deviceMessageDocument.Position.Heading = Convert.ToInt32(input.DataMessage.Reader.Get(3));
        deviceMessageDocument.Position.Speed = Convert.ToInt32(input.DataMessage.Reader.Get(3));
        deviceMessageDocument.Position.HDOP = float.Parse(input.DataMessage.Reader.Get(4));
        deviceMessageDocument.Device ??= new DeviceElement();
        deviceMessageDocument.Device.Odometer = uint.Parse(input.DataMessage.Reader.Get(7));
        deviceMessageDocument.Position.Latitude = GpsUtil.ConvertDmmLatToDecimal(input.DataMessage.Reader.Get(9),
            input.DataMessage.Reader.Get(1));
        deviceMessageDocument.Position.Longitude = GpsUtil.ConvertDmmLongToDecimal(input.DataMessage.Reader.Get(10),
            input.DataMessage.Reader.Get(1));

        return deviceMessageDocument;
    }

    private static DateTime ConvertDate(string date) => DateTimeUtil.New(date[..2], date[2..4], date[4..6],
        date[6..8], date[8..10], date[10..12]);
}