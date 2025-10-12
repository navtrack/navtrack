using System;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Devices;

namespace Navtrack.Api.Services.Devices.Mappers;

public static class DeviceDocumentMapper
{
    public static DeviceEntity Map(AssetEntity asset, Guid userId, string serialNumber, DeviceType deviceType)
    {
        return new DeviceEntity
        {
            AssetId = asset.Id,
            OrganizationId = asset.OrganizationId,
            SerialNumber = serialNumber,
            DeviceTypeId = deviceType.Id,
            ProtocolPort = deviceType.Protocol.Port,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = userId
        };
    }
}