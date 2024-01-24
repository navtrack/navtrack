using Navtrack.Api.Model.Locations;
using Navtrack.DataAccess.Model.New;

namespace Navtrack.Api.Services.Mappers.Locations;

public static class LocationMapper
{
    public static PositionModel Map(PositionElement source)
    {
        PositionModel position = new()
        {
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