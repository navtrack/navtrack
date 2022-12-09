using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.DataAccess.Mongo;
using Navtrack.Library.DI;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Services;

[Service(typeof(IConnectionService))]
public class ConnectionService : IConnectionService
{
    private readonly IRepository repository;

    public ConnectionService(IRepository repository)
    {
        this.repository = repository;
    }

    public async Task<DeviceConnectionDocument> NewConnection(string endPoint, int protocolPort)
    {
        DeviceConnectionDocument deviceConnection = new()
        {
            OpenedAt = DateTime.UtcNow,
            RemoteEndpoint = endPoint,
            ProtocolPort = protocolPort,
            Messages = new List<DeviceConnectionMessageElement>()
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

    public async Task SetDeviceId(Client client)
    {
        // TODO refactor this
        if (client.Device is { Entity: null } && !string.IsNullOrEmpty(client.Device.IMEI))
        {
            client.Device.Entity = await repository.GetEntities<AssetDocument>()
                .FirstOrDefaultAsync(
                    x => x.Device.SerialNumber == client.Device.IMEI &&
                         x.Device.ProtocolPort == client.Protocol.Port);

            if (client.Device.Entity != null)
            {
                await repository.GetCollection<DeviceConnectionDocument>()
                    .UpdateOneAsync(x => x.Id == client.DeviceConnection.Id,
                        Builders<DeviceConnectionDocument>.Update.Set(x => x.DeviceId,
                            client.Device.Entity.Device.Id));
            }
        }
    }
}