using MongoDB.Bson;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Services.Mappers.Common;
using Navtrack.DataAccess.Model.Devices;

namespace Navtrack.Api.Services.Mappers.Devices;

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

    public static DeviceDocument Map(string assetId, ChangeDeviceModel model, ObjectId userId)
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