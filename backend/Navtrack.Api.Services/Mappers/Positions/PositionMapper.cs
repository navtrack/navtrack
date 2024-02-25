using Navtrack.Api.Model.Positions;
using Navtrack.DataAccess.Model.Positions;

namespace Navtrack.Api.Services.Mappers.Positions;

public static class PositionMapper
{
    public static PositionModel Map(PositionDocument source)
    {
        PositionModel position = new()
        {
            Id = source.Id.ToString(),
            Coordinates = source.Coordinates,
            DateTime = source.Date,
            Speed = source.Speed,
            Heading = source.Heading,
            Altitude = source.Altitude,
            Satellites = source.Satellites,
            HDOP = source.HDOP,
            Valid = source.Valid,
            GsmSignal = source.GsmSignal,
            Odometer = source.Odometer
        };

        return position;
    }
}