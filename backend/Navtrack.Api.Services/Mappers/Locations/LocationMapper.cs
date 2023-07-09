using Navtrack.Api.Model.Locations;
using Navtrack.DataAccess.Model.Locations;

namespace Navtrack.Api.Services.Mappers.Locations;

public static class LocationMapper
{
    public static LocationModel Map(LocationDocument source)
    {
        LocationModel location = new()
        {
            Id = source.Id.ToString(),
            Latitude = source.Coordinates[1],
            Longitude = source.Coordinates[0],
            Coordinates = source.Coordinates,
            DateTime = source.DateTime,
            Speed = source.Speed,
            Heading = source.Heading,
            Altitude = source.Altitude,
            Satellites = source.Satellites,
            HDOP = source.HDOP,
            Valid = source.Valid,
            GsmSignal = source.GsmSignal,
            Odometer = source.Odometer
        };

        return location;
    }
}