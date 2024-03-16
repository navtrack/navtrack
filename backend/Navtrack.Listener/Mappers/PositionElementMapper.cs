using Navtrack.DataAccess.Model.Positions;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Mappers;

public static class PositionElementMapper
{
    public static PositionElement Map(Position source)
    {
        PositionElement destination = new()
        {
            Coordinates = [source.Longitude, source.Latitude],
            Date = source.Date,
            Valid = source.PositionStatus,
            Speed = source.Speed,
            Heading = source.Heading,
            Altitude = source.Altitude,
            Satellites = source.Satellites,
            HDOP = source.HDOP,
            Odometer = source.Odometer
        };

        return destination;
    }
}