using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Model.Filters;
using Navtrack.Database.Postgres;

namespace Navtrack.Database.Services.Devices;

public interface IDeviceMessageRepository: IGenericPostgresRepository<DeviceMessageEntity>
{
    Task<GetMessagesResult> GetMessages(GetMessagesOptions options);
    Task<Dictionary<Guid, int>> GetMessagesCountByDeviceIds(IEnumerable<Guid> deviceIds);
    Task DeleteByAssetId(Guid assetId);
    Task<bool> DeviceHasMessages(Guid assetId, Guid deviceId);
    Task<GetFirstAndLastPositionResult> GetFirstAndLast(Guid assetId,
        DateTime? startDate, DateTime? endDate);
    Task<GetFirstAndLastPositionResult> GetFirstAndLast(Guid assetId,
        DateTime date);
    Task<int?> GetFirstOdometer(Guid assetId);
}