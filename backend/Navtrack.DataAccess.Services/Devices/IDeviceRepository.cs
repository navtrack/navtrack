using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Services.Devices;

public interface IDeviceRepository : IGenericRepository<DeviceDocument>
{
    Task<bool> SerialNumberIsUsed(string serialNumber, int protocolPort, string? excludeAssetId = null);
    Task<List<DeviceDocument>> GetDevicesByAssetId(string assetId);
    Task<bool> IsActive(string assetId, string deviceId);
    Task DeleteByAssetId(ObjectId id);
}