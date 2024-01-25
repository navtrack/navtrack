using System;
using System.Linq;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Megastek;

[Service(typeof(ICustomMessageHandler<MegastekProtocol>))]
public class MegastekMessageHandler : BaseMessageHandler<MegastekProtocol>
{
    public override Position Parse(MessageInput input)
    {
        Position position = Parse(input, Parse_V1, Parse_V2, Parse_V3);

        return position;
    }

    private static Position Parse_V1(MessageInput input)
    {
        GPRMC gprmc = GPRMC.Parse(string.Join(",", input.DataMessage.CommaSplit.Skip(2).Take(13)));

        input.ConnectionContext.SetDevice(input.DataMessage.CommaSplit[17].Replace("imei:", string.Empty));
            
        Position position = new(gprmc)
        {
            Device = input.ConnectionContext.Device,
            Satellites = input.DataMessage.CommaSplit.Get<short>(18),
            Altitude = input.DataMessage.CommaSplit.Get<float?>(19)
        };

        return position;
    }

    private static Position Parse_V2(MessageInput input)
    {
        string imei = input.DataMessage.Reader.Skip(3).Get(16).Replace(" ", string.Empty);

        GPRMC gprmc = GPRMC.Parse(input.DataMessage.Reader.Skip(2).GetUntil('*', 3));

        input.ConnectionContext.SetDevice(imei);
            
        Position position = new(gprmc)
        {
            Device = input.ConnectionContext.Device,
            GsmSignal = input.DataMessage.CommaSplit.Get<short?>(17)
        };

        return position;
    }

    private static Position Parse_V3(MessageInput input)
    {
        input.ConnectionContext.SetDevice(input.DataMessage.CommaSplit[1]);
            
        Position position = new()
        {
            Device = input.ConnectionContext.Device,
            Latitude = GpsUtil.ConvertDmmLatToDecimal(input.DataMessage.CommaSplit[7],
                input.DataMessage.CommaSplit[8]),
            Longitude = GpsUtil.ConvertDmmLongToDecimal(input.DataMessage.CommaSplit[9],
                input.DataMessage.CommaSplit[10]),
            Date = GetDate(input.DataMessage.CommaSplit[4], input.DataMessage.CommaSplit[5]),
            Satellites = input.DataMessage.CommaSplit.Get<short?>(12),
            HDOP = input.DataMessage.CommaSplit.Get<float?>(14),
            Speed = SpeedUtil.KnotsToKph(input.DataMessage.CommaSplit.Get<float>(15)),
            Heading = input.DataMessage.CommaSplit.Get<float?>(16),
            Altitude = input.DataMessage.CommaSplit.Get<float?>(17),
            Odometer = input.DataMessage.CommaSplit.Get<double?>(18) * 1000,
            GsmSignal = input.DataMessage.CommaSplit.Get<short?>(23)
        };

        return position;
    }

    private static DateTime GetDate(string date, string time)
    {
        MessageReader dateReader = new(date);
        MessageReader timeReader = new(time);

        (string day, string month, string year) = (dateReader.Get(2), dateReader.Get(2), dateReader.Get(2));
        (string hour, string minute, string second) = (timeReader.Get(2), timeReader.Get(2), timeReader.Get(2));

        return DateTimeUtil.New(year, month, day, hour, minute, second);
    }
}