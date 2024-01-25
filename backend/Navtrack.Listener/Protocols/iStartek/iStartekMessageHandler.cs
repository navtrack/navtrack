using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.iStartek;

[Service(typeof(ICustomMessageHandler<iStartekProtocol>))]
// ReSharper disable once InconsistentNaming
public class iStartekMessageHandler : BaseMessageHandler<iStartekProtocol>
{
    public override Position Parse(MessageInput input)
    {
        string data = input.DataMessage.String[13..^4];
        GPRMC gprmc = GPRMC.Parse(data.Substring(0, data.IndexOf('|')));
            
        input.ConnectionContext.SetDevice(string.Join(string.Empty, input.DataMessage.Hex[4..11]).TrimEnd('F'));
            
        Position position = new(gprmc)
        {
            Device = input.ConnectionContext.Device,
            Heading = input.DataMessage.BarSplit.Get<float?>(1),
            Altitude = input.DataMessage.BarSplit.Get<float?>(2)
            // TODO add odometer
        };

        return position;
    }
}