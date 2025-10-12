using System;
using System.Threading.Tasks;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Postgres;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Database.Services.Devices;

[Service(typeof(IDeviceConnectionRepository))]
public class DeviceConnectionRepository : GenericPostgresRepository<DeviceConnectionEntity>, IDeviceConnectionRepository
{
    public DeviceConnectionRepository(IPostgresRepository repository) : base(repository)
    {
    }

    public Task AddMessage(Guid connectionId, byte[] hex)
    {
        // TODO
            return Task.CompletedTask;
        
        // return repository.GetCollection<DeviceConnectionDocument>()
        //     .UpdateOneAsync(x => x.Id == connectionId,
        //         Builders<DeviceConnectionDocument>.Update.Push(x => x.Messages, hex));
    }
}