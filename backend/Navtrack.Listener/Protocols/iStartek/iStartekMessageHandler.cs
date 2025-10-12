using Navtrack.Database.Model.Devices;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Listener.Services.Mappers;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.iStartek;

[Service(typeof(ICustomMessageHandler<iStartekProtocol>))]
// ReSharper disable once InconsistentNaming
public class iStartekMessageHandler : BaseMessageHandler<iStartekProtocol>
{
    public override DeviceMessageEntity? Parse(MessageInput input)
    {
        string data = input.DataMessage.String[13..^4];
        GPRMC? gprmc = GPRMC.Parse(data.Substring(0, data.IndexOf('|')));

        input.ConnectionContext.SetDevice(string.Join(string.Empty, input.DataMessage.Hex[4..11]).TrimEnd('F'));

        DeviceMessageEntity deviceMessage = new();
        DeviceMessageEntityMapper.Map(gprmc, deviceMessage);

        deviceMessage.Heading = input.DataMessage.BarSplit.Get<short?>(1);
        deviceMessage.Altitude = input.DataMessage.BarSplit.Get<short?>(2);

        return deviceMessage;
    }
}