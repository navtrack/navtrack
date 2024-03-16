using System;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Positions;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Mappers;

public static class MessageDocumentMapper
{
    public static MessageDocument Map(Position source, Device device, ObjectId connectionId)
    {
        MessageDocument destination = new()
        {
            ConnectionId = connectionId,
            Metadata = PositionMetadataElementMapper.Map(device),
            Position = PositionElementMapper.Map(source),
            Date = DateTime.UtcNow,
            Gsm = GsmElementMapper.Map(source)
        };

        return destination;
    }
}