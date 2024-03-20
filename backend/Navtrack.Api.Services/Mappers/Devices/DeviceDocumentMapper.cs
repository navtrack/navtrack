using System;
using MongoDB.Bson;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Devices;
using Navtrack.DataAccess.Model.Devices;

namespace Navtrack.Api.Services.Mappers.Devices;

public static class DeviceDocumentMapper
{
    public static DeviceDocument Map(ObjectId assetId, CreateAssetModel model, ObjectId userId)
    {
        return new DeviceDocument
        {
            AssetId = assetId,
            SerialNumber = model.SerialNumber,
            DeviceTypeId = model.DeviceTypeId,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = userId
        };
    }

    public static DeviceDocument Map(string assetId, CreateOrUpdateAssetDeviceModel model, ObjectId userId)
    {
        return new DeviceDocument
        {
            AssetId = ObjectId.Parse(assetId),
            SerialNumber = model.SerialNumber,
            DeviceTypeId = model.DeviceTypeId,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = userId
        };
    }
}