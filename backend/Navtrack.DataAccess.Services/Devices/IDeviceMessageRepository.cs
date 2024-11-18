using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.DataAccess.Model.Devices.Messages.Filters;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Services.Devices;

public interface IDeviceMessageRepository : IGenericRepository<DeviceMessageDocument>
{
    Task<GetMessagesResult> GetMessages(GetMessagesOptions options);
    Task<Dictionary<ObjectId, int>> GetMessagesCountByDeviceIds(IEnumerable<ObjectId> deviceIds);
    Task DeleteByAssetId(ObjectId assetId);
    Task<bool> DeviceHasMessages(string assetId, string deviceId);
    Task<GetFirstAndLastPositionResult> GetFirstAndLast(ObjectId assetId,
        DateTime? startDate, DateTime? endDate);
    Task<DeviceMessageDocument?> GetFirstOdometer(string assetId);
}