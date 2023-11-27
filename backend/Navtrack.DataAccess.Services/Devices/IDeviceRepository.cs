using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Services.Devices;

public interface IDeviceRepository : IGenericRepository<DeviceDocument>
{
    Task<bool> SerialNumberIsUsed(string serialNumber, int protocolPort, string excludeAssetId);
    Task<AssetDocument> GetActiveDeviceByDeviceId(string deviceId);
    Task<List<DeviceDocument>> GetDevicesByAssetId(string assetId);
    Task<bool> DeviceHasLocations(ObjectId deviceId);
    Task<bool> IsActive(string assetId, string deviceId);
}