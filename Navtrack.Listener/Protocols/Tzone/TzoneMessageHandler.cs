using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Tzone
{
    [Service(typeof(ICustomMessageHandler<TzoneProtocol>))]
    public class TzoneMessageHandler : BaseMessageHandler<TzoneProtocol>
    {
        public override Location Parse(MessageInput input)
        {
            GPRMC gprmc = new GPRMC(input.MessageData.StringBarSplit[1].Substring(2));

            Location location = new Location
            {
                Device = new Device
                {
                    IMEI = input.MessageData.StringBarSplit[0].Substring(4)
                },
                DateTime = gprmc.DateTime,
                Latitude = gprmc.Latitude,
                Longitude = gprmc.Longitude,
                PositionStatus = gprmc.PositionStatus,
                Speed = gprmc.Speed,
                Heading = gprmc.Heading,
                HDOP = input.MessageData.StringBarSplit.Get<double>(3),
                Odometer = input.MessageData.StringBarSplit.GetDouble(11, 4)
            };

            return location;
        }
    }
}