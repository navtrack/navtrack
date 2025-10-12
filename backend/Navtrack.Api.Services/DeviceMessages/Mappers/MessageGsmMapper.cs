using Navtrack.Api.Model.Messages;
using Navtrack.Database.Model.Devices;

namespace Navtrack.Api.Services.DeviceMessages.Mappers;

public static class MessageGsmMapper
{
    public static GsmDataModel? Map(DeviceMessageEntity source)
    {
        if (source.GSMSignalLevel.HasValue || !string.IsNullOrEmpty(source.GSMMobileCountryCode) ||
            !string.IsNullOrEmpty(source.GSMMobileNetworkCode))
        {
            GsmDataModel result = new()
            {
                SignalLevel = source.GSMSignalLevel,
                MobileCountryCode = source.GSMMobileCountryCode,
                MobileNetworkCode = source.GSMMobileNetworkCode
            };

            return result;
        }

        return null;
    }
}