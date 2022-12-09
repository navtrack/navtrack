using System.Text.RegularExpressions;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Bofan;

public class BaseBofanMessageHandler<T> : BaseMessageHandler<T>
{
    public override Location Parse(MessageInput input)
    {
        GPRMC gprmc = GPRMC.Parse(input.DataMessage.String);

        if (gprmc != null)
        {
            Match deviceIdMatch = new Regex(",(\\d+),(\\d{6}.\\d{3}),").Match(input.DataMessage.String);

            if (deviceIdMatch.Success)
            {
                input.Client.SetDevice(deviceIdMatch.Groups[1].Value);
                    
                Location location = new(gprmc)
                {
                    Device = input.Client.Device
                };

                return location;
            }
        }

        return null;
    }
}