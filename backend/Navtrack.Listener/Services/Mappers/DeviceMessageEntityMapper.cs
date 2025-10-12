using Navtrack.Database.Model.Devices;
using Navtrack.Listener.Helpers;

namespace Navtrack.Listener.Services.Mappers;

public static class DeviceMessageEntityMapper
{
    public static void Map(GPRMC? source, DeviceMessageEntity destination)
    {
        if (source == null) return;
        
        destination.Latitude = source.Latitude;
        destination.Longitude = source.Longitude;
        destination.Date = source.DateTime;
        destination.Valid = source.PositionStatus;
        destination.Speed = source.Speed.ToShort();
        destination.Heading = source.Heading.ToShort();
    }
}