using Navtrack.Api.Model.Messages;
using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Api.Services.DeviceMessages.Mappers;

public static class MessagePositionMapper
{
    public static MessagePosition Map(PositionElement source)
    {
        MessagePosition result = new()
        {
            Coordinates = new LatLong(source.Latitude, source.Longitude),
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