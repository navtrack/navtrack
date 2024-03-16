using Navtrack.Api.Model.Positions;
using Navtrack.DataAccess.Model.Positions;

namespace Navtrack.Api.Services.Mappers.Positions;

public static class GsmModelMapper
{
    public static GsmModel? Map(MessageDocument source)
    {
        if (source.Gsm != null)
        {
            GsmModel position = new()
            {
                GsmSignal = source.Gsm.Signal
            };

            return position;
        }

        return null;
    }
}