using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.DataAccess.Mongo;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.Listener.Models;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Services;

[Service(typeof(IConnectionService))]
public class ConnectionService(IRepository repository, IAssetRepository assetRepository) : IConnectionService
{
    public async Task<DeviceConnectionDocument> NewConnection(string endPoint, int protocolPort)
    {
        DeviceConnectionDocument deviceConnection = new()
        {
            OpenedAt = DateTime.UtcNow,
            RemoteEndpoint = endPoint,
            ProtocolPort = protocolPort,
            Messages = []
        };

        await repository.GetCollection<DeviceConnectionDocument>().InsertOneAsync(deviceConnection);
     
        return deviceConnection;
    }

    public Task MarkConnectionAsClosed(DeviceConnectionDocument deviceConnection)
    {
        return repository.GetCollection<DeviceConnectionDocument>()
            .UpdateOneAsync(x => x.Id == deviceConnection.Id,
                Builders<DeviceConnectionDocument>.Update.Set(x => x.ClosedAt, DateTime.UtcNow));
    }

    public async Task<ObjectId> AddMessage(ObjectId deviceConnectionId, string hex)
    {
        DeviceConnectionMessageElement deviceConnectionMessageElement = new()
        {
            Hex = hex
        };

        await repository.GetCollection<DeviceConnectionDocument>()
            .UpdateOneAsync(x => x.Id == deviceConnectionId,
                Builders<DeviceConnectionDocument>.Update.AddToSet(x => x.Messages,
                    deviceConnectionMessageElement));

        return deviceConnectionMessageElement.Id;
    }

    public async Task SetDeviceId(Device? device, int protocolPort)
    {
        if (device?.AssetId == null && !string.IsNullOrEmpty(device?.SerialNumber))
        {
            AssetDocument? asset = await assetRepository.Get(device.SerialNumber, protocolPort);
            
            if (asset != null)
            {
                device.AssetId = asset.Id;
                device.DeviceId = asset.Device.Id;
            }
        }
    }
}