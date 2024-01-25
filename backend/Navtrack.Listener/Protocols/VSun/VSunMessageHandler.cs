using System.Text.RegularExpressions;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.VSun;

[Service(typeof(ICustomMessageHandler<VSunProtocol>))]
public class VSunMessageHandler : BaseMessageHandler<VSunProtocol>
{
    public override Position Parse(MessageInput input)
    {
        Position position = Parse(input, HandleImei, HandleLocation);

        return position;
    }

    private static Position HandleLocation(MessageInput input)
    {
        GPRMC gprmc = GPRMC.Parse(input.DataMessage.String);

        if (gprmc != null)
        {
            Position position = new(gprmc)
            {
                Device = input.ConnectionContext.Device
            };

            return position;
        }
            
        return null;
    }

    private static Position HandleImei(MessageInput input)
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