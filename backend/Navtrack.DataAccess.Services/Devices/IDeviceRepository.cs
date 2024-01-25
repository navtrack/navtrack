using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Services.Devices;

public interface IDeviceRepository : IGenericRepository<DeviceDocument>
{
    Task<bool> SerialNumberIsUsed(string serialNumber, int protocolPort, string excludeAssetId);
    Task<List<DeviceDocument>> GetDevicesByAssetId(string assetId);
    Task<bool> IsActive(string assetId, string deviceId);
}