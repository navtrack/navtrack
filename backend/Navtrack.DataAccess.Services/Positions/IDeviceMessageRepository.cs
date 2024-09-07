using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.DataAccess.Model.Devices.Messages.Filters;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Services.Positions;

public interface IDeviceMessageRepository : IGenericRepository<DeviceMessageDocument>
{
    Task<GetMessagesResult> GetMessages(GetMessagesOptions options);
    Task<Dictionary<ObjectId, int>> GetMessagesCountByDeviceIds(IEnumerable<ObjectId> deviceIds);
    Task DeleteByAssetId(string assetId);
    Task<bool> DeviceHasMessages(string assetId, string deviceId);
    Task<(DeviceMessageDocument? first, DeviceMessageDocument? last)> GetFirstAndLastPosition(string assetId,
        DateTime startDate, DateTime endDate);
}