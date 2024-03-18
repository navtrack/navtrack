using System;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Mappers;

public static class MessageDocumentMapper
{
    public static MessageDocument Map(Position source, Device device, ObjectId connectionId)
    {
        MessageDocument destination = new()
        {
            ConnectionId = connectionId,
            CreatedDate = DateTime.UtcNow,
            Metadata = PositionMetadataElementMapper.Map(device),
            Position = PositionElementMapper.Map(source),
            Gsm = GsmElementMapper.Map(source)
        };

        return destination;
    }
}