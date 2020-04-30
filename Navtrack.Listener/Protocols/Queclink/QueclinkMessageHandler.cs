using System;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Queclink
{
    [Service(typeof(ICustomMessageHandler<QueclinkProtocol>))]
    public class QueclinkMessageHandler : BaseMessageHandler<QueclinkProtocol>
    {
        public override Location Parse(MessageInput input)
        {
            int? index = GetHDOPIndex(input.DataMessage.CommaSplit);

            if (index.HasValue)
            {
                Location location = new Location
                {
                    Device = new Device
                    {
                        IMEI = input.DataMessage.CommaSplit.Get<string>(2)
                    },
                    HDOP = input.DataMessage.CommaSplit.Get<double>(index.Value),
                    Speed = input.DataMessage.CommaSplit.Get<double>(index.Value+1),
                    Heading = input.DataMessage.CommaSplit.Get<float?>(index.Value+2),
                    Altitude = input.DataMessage.CommaSplit.Get<double?>(index.Value+3),
                    Longitude = input.DataMessage.CommaSplit.Get<decimal>(index.Value+4),
                    Latitude = input.DataMessage.CommaSplit.Get<decimal>(index.Value+5),
                    DateTime = ConvertDate(input.DataMessage.CommaSplit.Get<string>(index.Value+6))
                };
                location.PositionStatus = location.HDOP > 0;
                
                return location;
            }

            return null;
        }

        private static int? GetHDOPIndex(string[] message)
        {
            for (int i = 0; i < message.Length - 2; i++)
            {
                if (message[i].Contains(".") && message[i + 1].Contains(".") && message[i + 2].Length == 14)
                {
                    return i - 4;
                }
            }

            return null;
        }

        private static DateTime ConvertDate(string date) => DateTimeUtil.New(date[..4], date[4..6], date[6..8],
            date[8..10], date[10..12], date[12..14]);
    }
}