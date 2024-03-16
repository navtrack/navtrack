using Navtrack.Api.Model.Positions;
using Navtrack.DataAccess.Model.Positions;

namespace Navtrack.Api.Services.Mappers.Positions;

public static class PositionModelMapper
{
    public static PositionModel Map(MessageDocument source)
    {
        PositionModel position = new()
        {
            Id = source.Id.ToString(),
            Coordinates = source.Position.Coordinates,
            Date = source.Position.Date,
            Speed = source.Position.Speed,
            Heading = source.Position.Heading,
            Altitude = source.Position.Altitude,
            Satellites = source.Position.Satellites,
            HDOP = source.Position.HDOP,
            Valid = source.Position.Valid,
            Odometer = source.Position.Odometer,
            Gsm = GsmModelMapper.Map(source)
        };

        return position;
    }
}