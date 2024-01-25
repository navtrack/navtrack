using System;
using Navtrack.DataAccess.Model.Positions;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Mappers;

public static class PositionElementMapper
{
    public static PositionElement Map(Location source1)
    {
        PositionElement destination = new()
        {
            Coordinates = [source1.Longitude, source1.Latitude],
            Date = source1.Date,
            Valid = source1.PositionStatus,
            Speed = source1.Speed,
            Heading = source1.Heading,
            Altitude = source1.Altitude,
            Satellites = source1.Satellites,
            HDOP = source1.HDOP,
            GsmSignal = source1.GsmSignal,
            Odometer = source1.Odometer,
            CreatedDate = DateTime.UtcNow,
            CellGlobalIdentity = CellGlobalIdentityElementMapper.Map(source1)
        };

        return destination;
    }
}