using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using Navtrack.DataAccess.Model.Assets;
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
}