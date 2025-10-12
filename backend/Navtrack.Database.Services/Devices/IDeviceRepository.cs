using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Postgres;

namespace Navtrack.Database.Services.Devices;

public interface IDeviceRepository : IGenericPostgresRepository<DeviceEntity>
{
    Task<bool> SerialNumberIsUsed(string serialNumber, int protocolPort, Guid? excludeAssetId = null);
    Task<List<DeviceEntity>> GetDevicesByAssetId(Guid assetId);
    Task<bool> IsActive(Guid assetId, Guid deviceId);
    Task DeleteByAssetId(Guid id);
}