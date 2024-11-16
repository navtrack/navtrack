using System;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices;

namespace Navtrack.Api.Services.Devices.Mappers;

public static class DeviceDocumentMapper
{
    public static DeviceDocument Map(AssetDocument asset, ObjectId userId, string serialNumber, DeviceType deviceType)
    {
        return new DeviceDocument
        {
            AssetId = asset.Id,
            OrganizationId = asset.OrganizationId,
            SerialNumber = serialNumber,
            DeviceTypeId = deviceType.Id,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = userId
        };
    }
}