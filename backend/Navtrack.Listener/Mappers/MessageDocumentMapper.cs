using System;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Mappers;

public static class MessageDocumentMapper
{
    public static DeviceMessageDocument Map(Device device, ObjectId connectionId, DeviceMessageDocument destination)
    {
        destination.ConnectionId = connectionId;
        destination.CreatedDate = DateTime.UtcNow;
        destination.Metadata = PositionMetadataElementMapper.Map(device);

        if (destination.Data?.Count == 0)
        {
            destination.Data = null;
        }
        
        if (destination.Extra?.Count == 0)
        {
            destination.Extra = null;
        }


        return destination;
    }
}