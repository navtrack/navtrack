using System;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Totem;

[Service(typeof(ICustomMessageHandler<TotemProtocol>))]
public class TotemMessageHandler : BaseMessageHandler<TotemProtocol>
{
    public override Position Parse(MessageInput input)
    {
        Position position = Parse(input, ParseLocation_AT07, ParseLocation_AT09);

        return position;
    }

    private static Position ParseLocation_AT07(MessageInput input)
    {
        input.ConnectionContext.SetDevice(input.DataMessage.Reader.Skip(8).GetUntil('|'));
            
        return new Position
        {
            Device = input.ConnectionContext.Device,
            Date = ConvertDate(input.DataMessage.Reader.Skip(8).Get(12)),
            Satellites = Convert.ToInt16(input.DataMessage.Reader.Skip(16).Get(2)),
            GsmSignal = GsmUtil.ConvertSignal(Convert.ToInt16(input.DataMessage.Reader.Get(2))),
            Heading = Convert.ToInt32(input.DataMessage.Reader.Get(3)),
            Speed = Convert.ToInt32(input.DataMessage.Reader.Get(3)),
            HDOP = float.Parse(input.DataMessage.Reader.Get(4)),
            Odometer = uint.Parse(input.DataMessage.Reader.Get(7)),
            Latitude = GpsUtil.ConvertDmmLatToDecimal(input.DataMessage.Reader.Get(9),
                input.DataMessage.Reader.Get(1)),
            Longitude = GpsUtil.ConvertDmmLongToDecimal(input.DataMessage.Reader.Get(10),
                input.DataMessage.Reader.Get(1))
        };
    }

    private static Position ParseLocation_AT09(MessageInput input)
    {
        input.ConnectionContext.SetDevice(input.DataMessage.Reader.Skip(8).GetUntil('|'));
            
        return new Position
        {
            Device = input.ConnectionContext.Device,
            Date = ConvertDate(input.DataMessage.Reader.Skip(8).Get(12)),
            Satellites = Convert.ToInt16(input.DataMessage.Reader.Skip(36).Get(2)),
            GsmSignal = GsmUtil.ConvertSignal(Convert.ToInt16(input.DataMessage.Reader.Get(2))),
            Heading = Convert.ToInt32(input.DataMessage.Reader.Get(3)),
            Speed = Convert.ToInt32(input.DataMessage.Reader.Get(3)),
            HDOP = float.Parse(input.DataMessage.Reader.Get(4)),
            Odometer = uint.Parse(input.DataMessage.Reader.Get(7)),
            Latitude = GpsUtil.ConvertDmmLatToDecimal(input.DataMessage.Reader.Get(9),
                input.DataMessage.Reader.Get(1)),
            Longitude = GpsUtil.ConvertDmmLongToDecimal(input.DataMessage.Reader.Get(10),
                input.DataMessage.Reader.Get(1))
        };
    }

    private static DateTime ConvertDate(string date) => DateTimeUtil.New(date[..2], date[2..4], date[4..6],
        date[6..8], date[8..10], date[10..12]);
}