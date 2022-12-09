using System;
using System.Text.RegularExpressions;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Helpers.New2;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Laipac;

[Service(typeof(ICustomMessageHandler<LaipacProtocol>))]
public class LaipacMessageHandler : BaseMessageHandler<LaipacProtocol>
{
    private static string _pattern =
        "\\$AVRMC," +
        "(.*?)," + // device id
        "(\\d{6})," + // time hh mm ss
        "(.)," + // status
        "(\\d+.\\d+),(N|S)," + // latitude
        "(\\d+.\\d+),(E|W)," + // longitude
        "(\\d+.\\d+)," + // speed
        "(\\d+.\\d+)," + // heading
        "(\\d{6})," + // date dd mm yy
        "(.*?)," + // event
        "(.*?)," + // battery voltage
        "(.*?)," + // odometer
        "(.*?)," + // gps status
        "(.*?)," + // adc1
        "(.*?)\\*" + // adc2
        "(..)"; // checksum

    public override Location Parse(MessageInput input)
    {
        Location location = Parse(input, Authentication, Location);

        return location;
    }

    private static Location Authentication(MessageInput input)
    {
        if (input.DataMessage.String.Contains("$ECHK"))
        {
            input.NetworkStream.Write(input.DataMessage.Bytes);
        }

        return null;
    }

    private static Location Location(MessageInput input)
    {
        Match locationMatch =
            new Regex(
                    _pattern)
                .Match(input.DataMessage.String);

        if (locationMatch.Success)
        {
            input.Client.SetDevice(locationMatch.Groups[1].Value);
                
            Location location = new()
            {
                Device = input.Client.Device,
                DateTime = NewDateTimeUtil.Convert(DateFormat.DDMMYYHHMMSS,
                    $"{locationMatch.Groups[10].Value}{locationMatch.Groups[2].Value}"),
                Latitude = GpsUtil.ConvertDmmLatToDecimal(locationMatch.Groups[4].Value,
                    locationMatch.Groups[5].Value),
                Longitude = GpsUtil.ConvertDmmLongToDecimal(locationMatch.Groups[6].Value,
                    locationMatch.Groups[7].Value),
                Speed = SpeedUtil.KnotsToKph(locationMatch.Groups[8].Get<float>()),
                Heading = locationMatch.Groups[9].Get<float>()
            };

            SendLocationAcknowledge(locationMatch.Groups[3].Value, locationMatch.Groups[11].Value,
                locationMatch.Groups[17].Value, input);

            return location;
        }

        return null;
    }

    private static void SendLocationAcknowledge(string status, string @event, string checksum, MessageInput input)
    {
        if (Char.IsLower(status[0]))
        {
            string response = $"$EAVACK,{@event},{checksum}";
            response = $"{response}*{ChecksumUtil.NMEA(response.ToByteArray())}\r\n";

            input.NetworkStream.Write(StringUtil.ConvertStringToByteArray(response));
        }
    }
}