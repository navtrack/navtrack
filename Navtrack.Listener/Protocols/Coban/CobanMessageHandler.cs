using System;
using System.Text.RegularExpressions;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using static System.String;

namespace Navtrack.Listener.Protocols.Coban
{
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
                GroupCollection groups = new Regex(@"##,imei:(.*),A;").Matches(input.MessageData.String)[0].Groups;

                string imei = groups[1].Value;

                if (StringUtil.IsDigitsOnly(imei))
                {
                    input.Client.Device = new Device
                    {
                        IMEI = imei
                    };

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
            if (StringUtil.IsDigitsOnly(input.MessageData.String))
            {
                input.NetworkStream.Write(StringUtil.ConvertStringToByteArray("ON"));
            }

            return null;
        }

        private static Location Location(MessageInput input)
        {
            Location location = new Location
            {
                Device = new Device
                {
                    IMEI = input.MessageData.StringSplit.Get<string>(0).Replace("imei:", Empty)
                },
                DateTime = GetDate(input.MessageData.StringSplit.Get<string>(2)),
                PositionStatus = input.MessageData.StringSplit.Get<string>(4) == "F",
                Latitude = GpsUtil.ConvertDegreeAngleToDouble(@"(\d{2})(\d{2}).(\d{4})",
                    input.MessageData.StringSplit[7], input.MessageData.StringSplit[8]),
                Longitude = GpsUtil.ConvertDegreeAngleToDouble(@"(\d{3})(\d{2}).(\d{4})",
                    input.MessageData.StringSplit[9], input.MessageData.StringSplit[10]),
                Speed = input.MessageData.StringSplit.Get<double>(11),
                Heading = input.MessageData.StringSplit.Get<string>(12) != "1"
                    ? input.MessageData.StringSplit.Get<float?>(12)
                    : null,
                Altitude = input.MessageData.StringSplit.Get<double?>(13),
            };

            return location;
        }

        private static DateTime GetDate(string date)
        {
            MessageReader dateReader = new MessageReader(date);

            (string day, string month, string year, string hour, string minute) = (dateReader.Get(2), dateReader.Get(2),
                dateReader.Get(2), dateReader.Get(2), dateReader.Get(2));

            return DateTimeUtil.New(year, month, day, hour, minute, "0");
        }
    }
}