using System;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Meitrack
{
    [Service(typeof(ICustomMessageHandler<MeitrackProtocol>))]
    public class MeitrackMessageHandler : BaseMessageHandler<MeitrackProtocol>
    {
        public override Location Parse(MessageInput input)
        {
            Location location = new Location
            {
                Device = new Device
                {
                    IMEI = input.MessageData.StringSplit[1]
                },
                Latitude = input.MessageData.StringSplit.Get<decimal>(4),
                Longitude = input.MessageData.StringSplit.Get<decimal>(5),
                DateTime = ConvertDate(input.MessageData.StringSplit[6]),
                PositionStatus = input.MessageData.StringSplit.Get<string>(7) == "A",
                Satellites = input.MessageData.StringSplit.Get<short>(8),
                GsmSignal = input.MessageData.StringSplit.Get<short>(9),
                Speed = input.MessageData.StringSplit.Get<float>(10),
                Heading = input.MessageData.StringSplit.Get<float>(11),
                HDOP = input.MessageData.StringSplit.Get<float>(12),
                Altitude = input.MessageData.StringSplit.Get<int>(13),
                Odometer = input.MessageData.StringSplit.Get<uint>(14)
            };

            return location;
        }

        private static DateTime ConvertDate(string date) => DateTimeUtil.New(date[..2], date[2..4], date[4..6],
            date[6..8], date[8..10], date[10..12]);
    }
}