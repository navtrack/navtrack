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
            GPRMC gprmc = new GPRMC(input.DataMessage.BarSplit[1].Substring(2));

            Location location = new Location(gprmc)
            {
                Device = new Device
                {
                    IMEI = input.DataMessage.BarSplit[0].Substring(4)
                },
                HDOP = input.DataMessage.BarSplit.Get<decimal?>(3),
                Odometer = input.DataMessage.BarSplit.GetDouble(11, 4)
            };

            return location;
        }
    }
}