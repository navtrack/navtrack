using System;
using Navtrack.Database.Model.Devices;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Totem;

[Service(typeof(ICustomMessageHandler<TotemProtocol>))]
public class TotemMessageHandler : BaseMessageHandler<TotemProtocol>
{
    public override DeviceMessageEntity? Parse(MessageInput input)
    {
        DeviceMessageEntity deviceMessage = Parse(input, ParseLocation_AT07, ParseLocation_AT09);

        return deviceMessage;
    }

    private static DeviceMessageEntity ParseLocation_AT07(MessageInput input)
    {
        input.ConnectionContext.SetDevice(input.DataMessage.Reader.Skip(8).GetUntil('|'));

        DeviceMessageEntity deviceMessage = new();

        deviceMessage.Date = ConvertDate(input.DataMessage.Reader.Skip(8).Get(12));
        deviceMessage.Satellites = Convert.ToInt16(input.DataMessage.Reader.Skip(16).Get(2));
        deviceMessage.GSMSignalStrength = GsmUtil.ConvertSignal(Convert.ToInt16(input.DataMessage.Reader.Get(2)));
        deviceMessage.Heading = Convert.ToInt16(input.DataMessage.Reader.Get(3));
        deviceMessage.Speed = Convert.ToInt16(input.DataMessage.Reader.Get(3));
        deviceMessage.HDOP = float.Parse(input.DataMessage.Reader.Get(4));
        deviceMessage.DeviceOdometer = int.Parse(input.DataMessage.Reader.Get(7));
        deviceMessage.Latitude = GpsUtil.ConvertDmmLatToDecimal(input.DataMessage.Reader.Get(9),
            input.DataMessage.Reader.Get(1));
        deviceMessage.Longitude = GpsUtil.ConvertDmmLongToDecimal(input.DataMessage.Reader.Get(10),
            input.DataMessage.Reader.Get(1));
        
        return deviceMessage;
    }

    private static DeviceMessageEntity ParseLocation_AT09(MessageInput input)
    {
        input.ConnectionContext.SetDevice(input.DataMessage.Reader.Skip(8).GetUntil('|'));

        DeviceMessageEntity deviceMessage = new();

        deviceMessage.Date = ConvertDate(input.DataMessage.Reader.Skip(8).Get(12));
        deviceMessage.Satellites = Convert.ToInt16(input.DataMessage.Reader.Skip(36).Get(2));
        deviceMessage.GSMSignalStrength =
            GsmUtil.ConvertSignal(Convert.ToInt16(input.DataMessage.Reader.Get(2)));
        deviceMessage.Heading = Convert.ToInt16(input.DataMessage.Reader.Get(3));
        deviceMessage.Speed = Convert.ToInt16(input.DataMessage.Reader.Get(3));
        deviceMessage.HDOP = float.Parse(input.DataMessage.Reader.Get(4));
        deviceMessage.DeviceOdometer = int.Parse(input.DataMessage.Reader.Get(7));
        deviceMessage.Latitude = GpsUtil.ConvertDmmLatToDecimal(input.DataMessage.Reader.Get(9),
            input.DataMessage.Reader.Get(1));
        deviceMessage.Longitude = GpsUtil.ConvertDmmLongToDecimal(input.DataMessage.Reader.Get(10),
            input.DataMessage.Reader.Get(1));

        return deviceMessage;
    }

    private static DateTime ConvertDate(string date) => DateTimeUtil.New(date[..2], date[2..4], date[4..6],
        date[6..8], date[8..10], date[10..12]);
}