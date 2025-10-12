using System;
using System.Text.RegularExpressions;
using Navtrack.Database.Model.Devices;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;
using static System.String;

namespace Navtrack.Listener.Protocols.Coban;

[Service(typeof(ICustomMessageHandler<CobanProtocol>))]
public class CobanMessageHandler : BaseMessageHandler<CobanProtocol>
{
    public override DeviceMessageEntity? Parse(MessageInput input)
    {
        DeviceMessageEntity deviceMessageDocument = Parse(input, Authentication, Heartbeat, Location);

        return deviceMessageDocument;
    }

    private static DeviceMessageEntity? Authentication(MessageInput input)
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

    private static DeviceMessageEntity? Heartbeat(MessageInput input)
    {
        if (StringUtil.IsDigitsOnly(input.DataMessage.String))
        {
            input.NetworkStream.Write(StringUtil.ConvertStringToByteArray("ON"));
        }

        return null;
    }

    private static DeviceMessageEntity Location(MessageInput input)
    {
        input.ConnectionContext.SetDevice(input.DataMessage.CommaSplit.Get<string>(0).Replace("imei:", Empty));

        DeviceMessageEntity deviceMessage = new()
        {
            Date = GetDate(input.DataMessage.CommaSplit.Get<string>(2)),
            Valid = input.DataMessage.CommaSplit.Get<string>(4) == "F",
            Latitude = GpsUtil.ConvertDmmLatToDecimal(input.DataMessage.CommaSplit[7],
                input.DataMessage.CommaSplit[8]),
            Longitude = GpsUtil.ConvertDmmLongToDecimal(input.DataMessage.CommaSplit[9],
                input.DataMessage.CommaSplit[10]),
            Speed = SpeedUtil.KnotsToKph(input.DataMessage.CommaSplit.Get<float>(11)),
            Heading = input.DataMessage.CommaSplit.Get<string>(12) != "1"
                ? input.DataMessage.CommaSplit.Get<short?>(12)
                : null,
            Altitude = input.DataMessage.CommaSplit.Get<short?>(13)
        };
        
        return deviceMessage;
    }

    private static DateTime GetDate(string date)
    {
        MessageReader dateReader = new(date);

        (string day, string month, string year, string hour, string minute) = (dateReader.Get(2), dateReader.Get(2),
            dateReader.Get(2), dateReader.Get(2), dateReader.Get(2));

        return DateTimeUtil.New(year, month, day, hour, minute, "0");
    }
}