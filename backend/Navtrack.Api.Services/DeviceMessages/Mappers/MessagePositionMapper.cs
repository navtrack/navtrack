using Navtrack.Api.Model.Messages;
using Navtrack.Database.Model.Devices;

namespace Navtrack.Api.Services.DeviceMessages.Mappers;

public static class MessagePositionMapper
{
    public static PositionDataModel Map(DeviceMessageEntity source)
    {
        PositionDataModel result = new()
        {
            Coordinates = new LatLong(source.Coordinates.Y, source.Coordinates.X),
            Date = source.Date,
            Speed = source.Speed ?? 0,
            Heading = source.Heading ?? 0,
            Altitude = source.Altitude ?? 0,
            Satellites = source.Satellites ?? 0,
            HDOP = source.HDOP ?? 0,
            PDOP = source.PDOP ?? 0,
            Valid = source.Valid ?? false
        };

        return result;
    }
}