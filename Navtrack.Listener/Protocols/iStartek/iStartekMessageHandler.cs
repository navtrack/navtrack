using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.iStartek
{
    [Service(typeof(ICustomMessageHandler<iStartekProtocol>))]
    // ReSharper disable once InconsistentNaming
    public class iStartekMessageHandler : BaseMessageHandler<iStartekProtocol>
    {
        public override Location Parse(MessageInput input)
        {
            string data = input.DataMessage.String[13..^4];
            GPRMC gprmc = new GPRMC(data.Substring(0, data.IndexOf('|')));
            
            Location location = new Location(gprmc)
            {
                Device = new Device
                {
                    DeviceId = string.Join(string.Empty, input.DataMessage.Hex[4..11]).TrimEnd('F')
                },
                Heading = input.DataMessage.BarSplit.Get<decimal?>(1),
                Altitude = input.DataMessage.BarSplit.Get<decimal?>(2)
                //TODO add odometer
            };

            return location;
        }
    }
}