using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Tzone;

[Service(typeof(ICustomMessageHandler<TzoneProtocol>))]
public class TzoneMessageHandler : BaseMessageHandler<TzoneProtocol>
{
    public override Position Parse(MessageInput input)
    {
        GPRMC gprmc = GPRMC.Parse(input.DataMessage.BarSplit[1].Substring(2));

        input.ConnectionContext.SetDevice(input.DataMessage.BarSplit[0].Substring(4));
            
        Position position = new(gprmc)
        {
            Device = input.ConnectionContext.Device,
            HDOP = input.DataMessage.BarSplit.Get<float?>(3),
            Odometer = input.DataMessage.BarSplit.GetDouble(11, 4)
        };

        return position;
    }
}