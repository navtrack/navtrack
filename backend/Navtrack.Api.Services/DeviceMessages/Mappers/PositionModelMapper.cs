using Navtrack.Api.Model.Messages;
using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Api.Services.DeviceMessages.Mappers;

public static class PositionModelMapper
{
    public static MessagePositionModel Map(PositionElement source)
    {
        MessagePositionModel position = new()
        {
            Coordinates = new LatLongModel(source.Latitude, source.Longitude),
            Date = source.Date,
            Speed = source.Speed ?? 0,
            Heading = source.Heading ?? 0,
            Altitude = source.Altitude ?? 0,
            Satellites = source.Satellites ?? 0,
            HDOP = source.HDOP ?? 0,
            PDOP = source.PDOP ?? 0,
            Valid = source.Valid ?? false
        };

        return position;
    }
}