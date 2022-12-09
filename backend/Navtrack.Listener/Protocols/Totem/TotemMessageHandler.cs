using System;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Totem;

[Service(typeof(ICustomMessageHandler<TotemProtocol>))]
public class TotemMessageHandler : BaseMessageHandler<TotemProtocol>
{
    public override Location Parse(MessageInput input)
    {
        Location location = Parse(input, ParseLocation_AT07, ParseLocation_AT09);

        return location;
    }

    private static Location ParseLocation_AT07(MessageInput input)
    {
        input.Client.SetDevice(input.DataMessage.Reader.Skip(8).GetUntil('|'));
            
        return new Location
        {
            Device = input.Client.Device,
            DateTime = ConvertDate(input.DataMessage.Reader.Skip(8).Get(12)),
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

    private static Location ParseLocation_AT09(MessageInput input)
    {
        input.Client.SetDevice(input.DataMessage.Reader.Skip(8).GetUntil('|'));
            
        return new Location
        {
            Device = input.Client.Device,
            DateTime = ConvertDate(input.DataMessage.Reader.Skip(8).Get(12)),
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