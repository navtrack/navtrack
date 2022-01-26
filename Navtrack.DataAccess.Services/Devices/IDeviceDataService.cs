using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices;

namespace Navtrack.DataAccess.Services.Devices;

public interface IDeviceDataService
{
    Task<bool> SerialNumberIsUsed(string serialNumber, int protocolPort, string excludeAssetId);
    Task<AssetDocument> GetActiveDeviceByDeviceId(string deviceId);
    Task<List<DeviceDocument>> GetDevicesByAssetId(string assetId);
    Task<bool> DeviceHasLocations(ObjectId deviceId);
    Task Add(DeviceDocument document);
    Task Delete(string id);
    Task<bool> IsActive(string id);
}