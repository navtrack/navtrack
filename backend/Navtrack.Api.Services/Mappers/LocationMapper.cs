using Navtrack.Api.Model.Locations;
using Navtrack.Core.Model;
using Navtrack.DataAccess.Model.Common;
using Navtrack.DataAccess.Model.Locations;

namespace Navtrack.Api.Services.Mappers;

public static class LocationMapper
{
    public static LocationModel Map(LocationDocument source, UnitsType unitsType)
    {
        if (source == null)
        {
            return null;
        }
            
        LocationModel location = new()
        {
            Id = source.Id.ToString(),
            Latitude = source.Coordinates[1],
            Longitude = source.Coordinates[0],
            Coordinates = source.Coordinates,
            DateTime = source.DateTime,
            Speed = UnitsMapper.MapSpeed(source.Speed, unitsType),
            Heading = source.Heading,
            Altitude = UnitsMapper.MapDistance(source.Altitude, unitsType),
            Satellites = source.Satellites,
            HDOP = source.HDOP,
            Valid = source.Valid,
            GsmSignal = source.GsmSignal,
            Odometer = source.Odometer
        };

        return location;
    }
}