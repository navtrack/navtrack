using System;
using System.Linq;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Megastek
{
    [Service(typeof(ICustomMessageHandler<MegastekProtocol>))]
    public class MegastekMessageHandler : BaseMessageHandler<MegastekProtocol>
    {
        public override Location Parse(MessageInput input)
        {
            Location location = Parse(input, Parse_V1, Parse_V2, Parse_V3);

            return location;
        }

        private static Location Parse_V1(MessageInput input)
        {
            GPRMC gprmc = new GPRMC(string.Join(",", input.MessageData.StringSplit.Skip(2).Take(13)));

            Location location = new Location(gprmc)
            {
                Device = new Device
                {
                    IMEI = input.MessageData.StringSplit[17].Replace("imei:", string.Empty)
                },
                Satellites = input.MessageData.StringSplit.Get<short>(18),
                Altitude = input.MessageData.StringSplit.Get<double>(19)
            };

            return location;
        }

        private static Location Parse_V2(MessageInput input)
        {
            string imei = input.MessageData.Reader.Skip(3).Get(16).Replace(" ", string.Empty);

            GPRMC gprmc = new GPRMC(input.MessageData.Reader.Skip(2).GetUntil('*', 3));

            Location location = new Location(gprmc)
            {
                Device = new Device
                {
                    IMEI = imei
                },
                GsmSignal = input.MessageData.StringSplit.Get<short?>(17)
            };

            return location;
        }

        private static Location Parse_V3(MessageInput input)
        {
            Location location = new Location
            {
                Device = new Device
                {
                    IMEI = input.MessageData.StringSplit[1]
                },
                Latitude = GpsUtil.ConvertDegreeAngleToDouble(@"(\d{2})(\d{2}).(\d{4})",
                    input.MessageData.StringSplit[7], input.MessageData.StringSplit[8]),
                Longitude = GpsUtil.ConvertDegreeAngleToDouble(@"(\d{3})(\d{2}).(\d{4})",
                    input.MessageData.StringSplit[9], input.MessageData.StringSplit[10]),
                DateTime = GetDate(input.MessageData.StringSplit[4], input.MessageData.StringSplit[5]),
                Satellites = input.MessageData.StringSplit.Get<short?>(12),
                HDOP = input.MessageData.StringSplit.Get<double>(14),
                Speed = input.MessageData.StringSplit.Get<double>(15) * 1.852,
                Heading = input.MessageData.StringSplit.Get<float?>(16),
                Altitude = input.MessageData.StringSplit.Get<double?>(17),
                Odometer = input.MessageData.StringSplit.Get<double?>(18)*1000,
                GsmSignal =  input.MessageData.StringSplit.Get<short?>(23)
            };

            return location;
        }

        private static DateTime GetDate(string date, string time)
        {
            MessageReader dateReader = new MessageReader(date);
            MessageReader timeReader = new MessageReader(time);

            (string day, string month, string year) = (dateReader.Get(2), dateReader.Get(2), dateReader.Get(2));
            (string hour, string minute, string second) = (timeReader.Get(2), timeReader.Get(2), timeReader.Get(2));

            return DateTimeUtil.New(year, month, day, hour, minute, second);
        }
    }
}