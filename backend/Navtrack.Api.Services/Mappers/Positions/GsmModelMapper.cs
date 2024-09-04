using Navtrack.Api.Model.Positions;
using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Api.Services.Mappers.Positions;

public static class GsmModelMapper
{
    public static GsmModel? Map(DeviceMessageDocument source)
    {
        if (source.Gsm != null)
        {
            GsmModel position = new()
            {
                GsmSignal = source.Gsm.SignalStrength
            };

            return position;
        }

        return null;
    }
}