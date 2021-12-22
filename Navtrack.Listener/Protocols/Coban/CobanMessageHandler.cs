using System;
using System.Text.RegularExpressions;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using static System.String;

namespace Navtrack.Listener.Protocols.Coban;

[Service(typeof(ICustomMessageHandler<CobanProtocol>))]
public class CobanMessageHandler : BaseMessageHandler<CobanProtocol>
{
    public override Location Parse(MessageInput input)
    {
        Location location = Parse(input, Authentication, Heartbeat, Location);

        return location;
    }

    private static Location Authentication(MessageInput input)
    {
        try
        {
            GroupCollection groups = new Regex(@"##,imei:(.*),A;").Matches(input.DataMessage.String)[0].Groups;

            string imei = groups[1].Value;

            if (StringUtil.IsDigitsOnly(imei))
            {
                input.Client.SetDevice(imei);

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

    private static Location Heartbeat(MessageInput input)
    {
        if (StringUtil.IsDigitsOnly(input.DataMessage.String))
        {
            input.NetworkStream.Write(StringUtil.ConvertStringToByteArray("ON"));
        }

        return null;
    }

    private static Location Location(MessageInput input)
    {
        input.Client.SetDevice(input.DataMessage.CommaSplit.Get<string>(0).Replace("imei:", Empty));

        Location location = new()
        {
            Device = input.Client.Device,
            DateTime = GetDate(input.DataMessage.CommaSplit.Get<string>(2)),
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

        return location;
    }

    private static DateTime GetDate(string date)
    {
        MessageReader dateReader = new(date);

        (string day, string month, string year, string hour, string minute) = (dateReader.Get(2), dateReader.Get(2),
            dateReader.Get(2), dateReader.Get(2), dateReader.Get(2));

        return DateTimeUtil.New(year, month, day, hour, minute, "0");
    }
}