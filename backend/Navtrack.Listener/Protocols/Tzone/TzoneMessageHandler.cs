using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Tzone;

[Service(typeof(ICustomMessageHandler<TzoneProtocol>))]
public class TzoneMessageHandler : BaseMessageHandler<TzoneProtocol>
{
    public override Location Parse(MessageInput input)
    {
        GPRMC gprmc = GPRMC.Parse(input.DataMessage.BarSplit[1].Substring(2));

        input.Client.SetDevice(input.DataMessage.BarSplit[0].Substring(4));
            
        Location location = new(gprmc)
        {
            Device = input.Client.Device,
            HDOP = input.DataMessage.BarSplit.Get<float?>(3),
            Odometer = input.DataMessage.BarSplit.GetDouble(11, 4)
        };

        return location;
    }
}