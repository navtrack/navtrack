using System;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.SinoTrack
{
    [Service(typeof(ICustomMessageHandler<SinoTrackProtocol>))]
    public class SinoTrackMessageHandler : BaseMessageHandler<SinoTrackProtocol>
    {
        public override Location Parse(MessageInput input)
        {
            Location location = new Location
            {
                Device = new Device
                {
                    IMEI = input.DataMessage.CommaSplit.Get<string>(1)
                },
                DateTime = ConvertDate(input.DataMessage.CommaSplit.Get<string>(3),
                    input.DataMessage.CommaSplit.Get<string>(11)),
                PositionStatus = input.DataMessage.CommaSplit.Get<string>(4) == "A",
                Latitude = GpsUtil.ConvertDmmLatToDecimal(input.DataMessage.CommaSplit[5],
                    input.DataMessage.CommaSplit[6]),
                Longitude = GpsUtil.ConvertDmmLongToDecimal(input.DataMessage.CommaSplit[7],
                    input.DataMessage.CommaSplit[8]),
                Speed = input.DataMessage.CommaSplit.Get<decimal?>(9) * (decimal) 1.852,
                Heading = input.DataMessage.CommaSplit.Get<decimal?>(10),
                Altitude = 0
            };

            return location;
        }

        private static DateTime ConvertDate(string time, string date) =>
            DateTimeUtil.New(date[4..6], date[2..4], date[..2], time[..2], time[2..4], time[4..6]);
    }
}