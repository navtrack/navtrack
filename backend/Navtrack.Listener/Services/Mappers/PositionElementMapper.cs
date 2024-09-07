using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;

namespace Navtrack.Listener.Services.Mappers;

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

    public static PositionElement Map(PositionElement destinationPosition)
    {
        destinationPosition.Speed = destinationPosition.Speed == 0 ? null : destinationPosition.Speed;
        destinationPosition.Heading = destinationPosition.Heading == 0 ? null : destinationPosition.Heading;
        destinationPosition.Altitude = destinationPosition.Altitude == 0 ? null : destinationPosition.Altitude;
        destinationPosition.Satellites = destinationPosition.Satellites == 0 ? null : destinationPosition.Satellites;
        destinationPosition.PDOP = destinationPosition.PDOP == 0 ? null : destinationPosition.PDOP;
        destinationPosition.HDOP = destinationPosition.HDOP == 0 ? null : destinationPosition.HDOP;
        destinationPosition.Valid = destinationPosition.Valid == false ? null : destinationPosition.Valid;
        
        return destinationPosition;
    }
}
