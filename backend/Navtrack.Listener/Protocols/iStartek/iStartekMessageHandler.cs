using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Mappers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.iStartek;

[Service(typeof(ICustomMessageHandler<iStartekProtocol>))]
// ReSharper disable once InconsistentNaming
public class iStartekMessageHandler : BaseMessageHandler<iStartekProtocol>
{
    public override DeviceMessageDocument Parse(MessageInput input)
    {
        string data = input.DataMessage.String[13..^4];
        GPRMC gprmc = GPRMC.Parse(data.Substring(0, data.IndexOf('|')));

        input.ConnectionContext.SetDevice(string.Join(string.Empty, input.DataMessage.Hex[4..11]).TrimEnd('F'));

        DeviceMessageDocument deviceMessageDocument = new()
        {
            Position = PositionElementMapper.Map(gprmc),
            // Device = input.ConnectionContext.Device,
        };

        deviceMessageDocument.Position.Heading = input.DataMessage.BarSplit.Get<float?>(1);
        deviceMessageDocument.Position.Altitude = input.DataMessage.BarSplit.Get<float?>(2);
        // TODO add odometer

        return deviceMessageDocument;
    }
}