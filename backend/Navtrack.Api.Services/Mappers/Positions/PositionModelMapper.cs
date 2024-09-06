using Navtrack.Api.Model.Positions;
using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Api.Services.Mappers.Positions;

public static class PositionModelMapper
{
    public static PositionModel Map(DeviceMessageDocument source)
    {
        PositionModel position = new()
        {
            Id = source.Id.ToString(),
            Coordinates = new LatLongModel(source.Position.Latitude, source.Position.Longitude),
            Date = source.Position.Date,
            Speed = source.Position.Speed,
            Heading = source.Position.Heading,
            Altitude = source.Position.Altitude,
            Satellites = source.Position.Satellites,
            HDOP = source.Position.HDOP,
            Valid = source.Position.Valid,
            Gsm = GsmModelMapper.Map(source)
        };

        return position;
    }
}