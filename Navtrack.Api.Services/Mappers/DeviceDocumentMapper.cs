using System;
using MongoDB.Bson;
using Navtrack.Api.Model.Assets;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Mappers;

public static class DeviceDocumentMapper
{
    public static DeviceDocument Map(AddAssetModel model, UserDocument user, DeviceType deviceType)
    {
        return new DeviceDocument
        {
            Id = ObjectId.GenerateNewId(),
            SerialNumber = model.SerialNumber,
            DeviceTypeId = Convert.ToInt32(model.DeviceTypeId),
            Created = AuditElementMapper.Map(user.Id),
            ProtocolPort = deviceType.Protocol.Port
        };
    }
}