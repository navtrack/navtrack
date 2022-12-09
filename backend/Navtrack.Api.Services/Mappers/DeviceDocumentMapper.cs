using System;
using MongoDB.Bson;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Devices;
using Navtrack.DataAccess.Model.Devices;

namespace Navtrack.Api.Services.Mappers;

public static class DeviceDocumentMapper
{
    public static DeviceDocument Map(AddAssetModel model, ObjectId assetId, ObjectId userId)
    {
        return new DeviceDocument
        {
            Id = ObjectId.GenerateNewId(),
            AssetId = assetId,
            SerialNumber = model.SerialNumber,
            DeviceTypeId = model.DeviceTypeId,
            Created = AuditElementMapper.Map(userId)
        };
    }

    public static DeviceDocument Map(string assetId, AddDeviceModel model, ObjectId userId)
    {
        return new DeviceDocument
        {
            Id = ObjectId.GenerateNewId(),
            AssetId = ObjectId.Parse(assetId),
            SerialNumber = model.SerialNumber,
            DeviceTypeId = model.DeviceTypeId,
            Created = AuditElementMapper.Map(userId)
        };
    }
}