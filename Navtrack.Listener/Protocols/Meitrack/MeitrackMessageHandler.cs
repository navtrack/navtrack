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
                    IMEI = input.DataMessage.CommaSplit[1]
                },
                Latitude = input.DataMessage.CommaSplit.Get<decimal>(4),
                Longitude = input.DataMessage.CommaSplit.Get<decimal>(5),
                DateTime = ConvertDate(input.DataMessage.CommaSplit[6]),
                PositionStatus = input.DataMessage.CommaSplit.Get<string>(7) == "A",
                Satellites = input.DataMessage.CommaSplit.Get<short>(8),
                GsmSignal = GsmUtil.ConvertSignal(input.DataMessage.CommaSplit.Get<short>(9)),
                Speed = input.DataMessage.CommaSplit.Get<decimal?>(10),
                Heading = input.DataMessage.CommaSplit.Get<decimal?>(11),
                HDOP = input.DataMessage.CommaSplit.Get<decimal?>(12),
                Altitude = input.DataMessage.CommaSplit.Get<int>(13),
                Odometer = input.DataMessage.CommaSplit.Get<uint>(14)
            };

            return location;
        }

        private static DateTime ConvertDate(string date) => DateTimeUtil.New(date[..2], date[2..4], date[4..6],
            date[6..8], date[8..10], date[10..12]);
    }
}