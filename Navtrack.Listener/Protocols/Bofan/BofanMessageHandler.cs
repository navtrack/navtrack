using System.Text.RegularExpressions;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Bofan
{
    [Service(typeof(ICustomMessageHandler<BofanProtocol>))]
    public class BofanMessageHandler : BaseMessageHandler<BofanProtocol>
    {
        public override Location Parse(MessageInput input)
        {
            GPRMC gprmc = GPRMC.Parse(input.DataMessage.String);

            if (gprmc != null)
            {
                Match deviceIdMatch = new Regex(",(\\d+),(\\d{6}.\\d{3}),").Match(input.DataMessage.String);

                if (deviceIdMatch.Success)
                {
                    Location location = new Location(gprmc)
                    {
                        Device = new Device {IMEI = deviceIdMatch.Groups[1].Value}
                    };

                    return location;
                }
            }

            return null;
        }
    }
}