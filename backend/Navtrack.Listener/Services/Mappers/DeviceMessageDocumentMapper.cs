using System;
using Navtrack.Database.Model.Devices;

namespace Navtrack.Listener.Services.Mappers;

public static class DeviceMessageDocumentMapper
{
    public static DeviceMessageEntity Map(Guid assetId, Guid deviceId, Guid connectionId,
        DeviceMessageEntity destination)
    {
        destination.ConnectionId = connectionId;
        destination.CreatedDate = DateTime.UtcNow;
        destination.AssetId = assetId;
        destination.DeviceId = deviceId;
        
        // DeviceMessageEntity cleaned = Clean(destination);

        return destination;
    }

    private static DeviceMessageEntity Clean(DeviceMessageEntity destination)
    {
        // destination.Position = DeviceMessageEntityMapper.Map(destination.Position);
        // destination.Device = DeviceElementMapper.Map(destination.Device);
        // destination.Vehicle = VehicleElementMapper.Map(destination.Vehicle);
        // destination.Gsm = GsmElementMapper.Map(destination.Gsm);
        //
        // if (destination.AdditionalData?.Count == 0)
        // {
        //     destination.AdditionalData = null;
        // }
        //
        // if (destination.AdditionalDataUnhandled?.Count == 0)
        // {
        //     destination.AdditionalDataUnhandled = null;
        // }

        return destination;
    }
}