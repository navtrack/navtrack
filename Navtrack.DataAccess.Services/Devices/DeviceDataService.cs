using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.DataAccess.Model.Locations;
using Navtrack.DataAccess.Mongo;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Services.Devices;

[Service(typeof(IDeviceDataService))]
public class DeviceDataService : IDeviceDataService
{
    private readonly IRepository repository;

    public DeviceDataService(IRepository repository)
    {
        this.repository = repository;
    }

    public Task<bool> SerialNumberIsUsed(string serialNumber, int protocolPort, string excludeAssetId = null)
    {
        Expression<Func<AssetDocument, bool>> filter = excludeAssetId == null
            ? x =>
                x.Device.SerialNumber == serialNumber &&
                x.Device.ProtocolPort == protocolPort
            : x =>
                x.Device.SerialNumber == serialNumber &&
                x.Device.ProtocolPort == protocolPort &&
                (excludeAssetId != null || x.Id != ObjectId.Parse(excludeAssetId));

        return repository.GetEntities<AssetDocument>()
            .AnyAsync(filter);
    }

    public Task<AssetDocument> GetActiveDeviceByDeviceId(string deviceId)
    {
        return repository.GetEntities<AssetDocument>()
            .FirstOrDefaultAsync(x => x.Device.SerialNumber == deviceId);
    }

    public Task<List<DeviceDocument>> GetDevicesByAssetId(string assetId)
    {
        return repository.GetEntities<DeviceDocument>().Where(x => x.AssetId == ObjectId.Parse(assetId)).ToListAsync();
    }

    public Task<bool> DeviceHasLocations(ObjectId deviceId)
    {
        return repository.GetEntities<LocationDocument>().AnyAsync(x => x.DeviceId == deviceId);
    }

    public Task Add(DeviceDocument document)
    {
        return repository.GetCollection<DeviceDocument>().InsertOneAsync(document);
    }

    public Task Delete(string id)
    {
        return repository.GetCollection<DeviceDocument>().DeleteOneAsync(x => x.Id == ObjectId.Parse(id));
    }

    public Task<bool> IsActive(string id)
    {
        return repository.GetEntities<AssetDocument>().AnyAsync(x => x.Device.Id == ObjectId.Parse(id));
    }
}