using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.DataAccess.Model.Locations;
using Navtrack.DataAccess.Mongo;
using Navtrack.Shared.Library.DI;

namespace Navtrack.DataAccess.Services.Devices;

[Service(typeof(IDeviceRepository))]
public class DeviceRepository(IRepository repository) : GenericRepository<DeviceDocument>(repository), IDeviceRepository
{
    public Task<bool> SerialNumberIsUsed(string serialNumber, int protocolPort, string? excludeAssetId = null)
    {
        return repository.GetQueryable<AssetDocument>()
            .AnyAsync(x => x.Device.SerialNumber == serialNumber &&
                           x.Device.ProtocolPort == protocolPort &&
                           (excludeAssetId == null || x.Id != ObjectId.Parse(excludeAssetId)));
    }

    public Task<AssetDocument> GetActiveDeviceByDeviceId(string deviceId)
    {
        return repository.GetQueryable<AssetDocument>()
            .FirstOrDefaultAsync(x => x.Device.SerialNumber == deviceId);
    }

    public Task<List<DeviceDocument>> GetDevicesByAssetId(string assetId)
    {
        return repository.GetQueryable<DeviceDocument>().Where(x => x.AssetId == ObjectId.Parse(assetId)).ToListAsync();
    }

    public Task<bool> DeviceHasLocations(ObjectId deviceId)
    {
        return repository.GetQueryable<LocationDocument>().AnyAsync(x => x.DeviceId == deviceId);
    }

    public Task<bool> IsActive(string assetId, string deviceId)
    {
        return repository.GetQueryable<AssetDocument>().AnyAsync(x => x.Device.Id == ObjectId.Parse(deviceId) &&
                                                                      x.Id == ObjectId.Parse(assetId));
    }
}