using System;
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
                HDOP = input.MessageData.StringSplit.Get<double>(14),
                Speed = (int) (Convert.ToDouble(input.MessageData.StringSplit[15])*1.852),
                Heading = (int) Convert.ToDouble(input.MessageData.StringSplit[16]),
                Altitude =  (int) Convert.ToDouble(input.MessageData.StringSplit[17])
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