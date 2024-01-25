using System;
using System.Text.RegularExpressions;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;
using static System.String;

namespace Navtrack.Listener.Protocols.Coban;

[Service(typeof(ICustomMessageHandler<CobanProtocol>))]
public class CobanMessageHandler : BaseMessageHandler<CobanProtocol>
{
    public override Position Parse(MessageInput input)
    {
        Position position = Parse(input, Authentication, Heartbeat, Location);

        return position;
    }

    private static Position Authentication(MessageInput input)
    {
        try
        {
            GroupCollection groups = new Regex(@"##,imei:(.*),A;").Matches(input.DataMessage.String)[0].Groups;

            string imei = groups[1].Value;

            if (StringUtil.IsDigitsOnly(imei))
            {
                input.ConnectionContext.SetDevice(imei);

                input.NetworkStream.Write(StringUtil.ConvertStringToByteArray("LOAD"));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return null;
    }

    private static Position Heartbeat(MessageInput input)
    {
        if (StringUtil.IsDigitsOnly(input.DataMessage.String))
        {
            input.NetworkStream.Write(StringUtil.ConvertStringToByteArray("ON"));
        }

        return null;
    }

    private static Position Location(MessageInput input)
    {
        input.ConnectionContext.SetDevice(input.DataMessage.CommaSplit.Get<string>(0).Replace("imei:", Empty));

        Position position = new()
        {
            Device = input.ConnectionContext.Device,
            Date = GetDate(input.DataMessage.CommaSplit.Get<string>(2)),
            PositionStatus = input.DataMessage.CommaSplit.Get<string>(4) == "F",
            Latitude = GpsUtil.ConvertDmmLatToDecimal(input.DataMessage.CommaSplit[7],
                input.DataMessage.CommaSplit[8]),
            Longitude = GpsUtil.ConvertDmmLongToDecimal(input.DataMessage.CommaSplit[9],
                input.DataMessage.CommaSplit[10]),
            Speed = SpeedUtil.KnotsToKph(input.DataMessage.CommaSplit.Get<float>(11)),
            Heading = input.DataMessage.CommaSplit.Get<string>(12) != "1"
                ? input.DataMessage.CommaSplit.Get<float?>(12)
                : null,
            Altitude = input.DataMessage.CommaSplit.Get<float?>(13),
        };

        return position;
    }

    private static DateTime GetDate(string date)
    {
        MessageReader dateReader = new(date);

        (string day, string month, string year, string hour, string minute) = (dateReader.Get(2), dateReader.Get(2),
            dateReader.Get(2), dateReader.Get(2), dateReader.Get(2));

        return DateTimeUtil.New(year, month, day, hour, minute, "0");
    }
}