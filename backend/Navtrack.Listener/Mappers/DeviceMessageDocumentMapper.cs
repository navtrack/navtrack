using System;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Mappers;

public static class DeviceMessageDocumentMapper
{
    public static DeviceMessageDocument Map(Device device, ObjectId connectionId, DeviceMessageDocument destination)
    {
        destination.ConnectionId = connectionId;
        destination.CreatedDate = DateTime.UtcNow;
        destination.Metadata = PositionMetadataElementMapper.Map(device);
        
        DeviceMessageDocument cleaned = Clean(destination);

        return cleaned;
    }
    
    public static DeviceMessageDocument Clean(DeviceMessageDocument destination)
    {
        destination.Position = PositionElementMapper.Map(destination.Position);
        destination.Device = DeviceElementMapper.Map(destination.Device);
        destination.Vehicle = VehicleElementMapper.Map(destination.Vehicle);
        destination.Gsm = GsmElementMapper.Map(destination.Gsm);

        if (destination.AdditionalData?.Count == 0)
        {
            destination.AdditionalData = null;
        }

        if (destination.AdditionalDataUnhandled?.Count == 0)
        {
            destination.AdditionalDataUnhandled = null;
        }

        return destination;
    }
}