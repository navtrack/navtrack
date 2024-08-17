using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;

namespace Navtrack.Listener.Mappers;

public static class PositionElementMapper
{
    public static PositionElement Map(GPRMC source)
    {
        PositionElement destination = new()
        {
            Coordinates = [source.Longitude, source.Latitude],
            Date = source.DateTime,
            Valid = source.PositionStatus,
            Speed = source.Speed,
            Heading = source.Heading
        };

        return destination;
    }
}
