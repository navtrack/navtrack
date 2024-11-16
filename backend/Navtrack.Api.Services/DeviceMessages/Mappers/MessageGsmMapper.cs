using Navtrack.Api.Model.Messages;
using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Api.Services.DeviceMessages.Mappers;

public static class MessageGsmMapper
{
    public static MessageGsm? Map(GsmElement? source)
    {
        if (source != null)
        {
            MessageGsm result = new()
            {
                SignalLevel = source.SignalLevel,
                MobileCountryCode = source.MobileCountryCode,
                MobileNetworkCode = source.MobileNetworkCode
            };

            return result;
        }

        return null;
    }
}