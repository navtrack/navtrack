using System;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Positions;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Mappers;

public static class PositionDocumentMapper
{
    public static PositionDocument Map(Position source, Device device, ObjectId connectionId)
    {
        PositionDocument destination = new()
        {
            ConnectionId = connectionId,
            Metadata = PositionMetadataElementMapper.Map(device),
            Coordinates = [source.Longitude, source.Latitude],
            Date = source.Date,
            Valid = source.PositionStatus,
            Speed = source.Speed,
            Heading = source.Heading,
            Altitude = source.Altitude,
            Satellites = source.Satellites,
            HDOP = source.HDOP,
            GsmSignal = source.GsmSignal,
            Odometer = source.Odometer,
            CreatedDate = DateTime.UtcNow,
            CellGlobalIdentity = CellGlobalIdentityElementMapper.Map(source)
        };

        return destination;
    }
}