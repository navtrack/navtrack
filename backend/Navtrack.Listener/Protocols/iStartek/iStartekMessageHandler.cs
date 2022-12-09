using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.iStartek;

[Service(typeof(ICustomMessageHandler<iStartekProtocol>))]
// ReSharper disable once InconsistentNaming
public class iStartekMessageHandler : BaseMessageHandler<iStartekProtocol>
{
    public override Location Parse(MessageInput input)
    {
        string data = input.DataMessage.String[13..^4];
        GPRMC gprmc = GPRMC.Parse(data.Substring(0, data.IndexOf('|')));
            
        input.Client.SetDevice(string.Join(string.Empty, input.DataMessage.Hex[4..11]).TrimEnd('F'));
            
        Location location = new(gprmc)
        {
            Device = input.Client.Device,
            Heading = input.DataMessage.BarSplit.Get<float?>(1),
            Altitude = input.DataMessage.BarSplit.Get<float?>(2)
            // TODO add odometer
        };

        return location;
    }
}