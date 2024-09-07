using Navtrack.Api.Model.Messages;
using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Api.Services.Mappers.Messages;

public static class GsmModelMapper
{
    public static MessageGsmModel? Map(GsmElement? source)
    {
        if (source != null)
        {
            MessageGsmModel model = new()
            {
                SignalLevel = source.SignalLevel,
                MobileCountryCode = source.MobileCountryCode,
                MobileNetworkCode = source.MobileNetworkCode
            };

            return model;
        }

        return null;
    }
}