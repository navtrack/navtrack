using System.Text.RegularExpressions;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.VSun;

[Service(typeof(ICustomMessageHandler<VSunProtocol>))]
public class VSunMessageHandler : BaseMessageHandler<VSunProtocol>
{
    public override Location Parse(MessageInput input)
    {
        Location location = Parse(input, HandleImei, HandleLocation);

        return location;
    }

    private static Location HandleLocation(MessageInput input)
    {
        GPRMC gprmc = GPRMC.Parse(input.DataMessage.String);

        if (gprmc != null)
        {
            Location location = new(gprmc)
            {
                Device = input.ConnectionContext.Device
            };

            return location;
        }
            
        return null;
    }

    private static Location HandleImei(MessageInput input)
    {
        if (input.ConnectionContext.Device == null)
        {
            Match locationMatch =
                new Regex("#(\\d{15})#" + // imei
                          "(.*?)#") // device model
                    .Match(input.DataMessage.String);

            if (locationMatch.Success)
            {
                input.ConnectionContext.SetDevice(locationMatch.Groups[1].Value);
            }
        }

        return null;
    }
}