using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Navtrack.DataAccess.Model.Devices.Connections;
using Navtrack.DataAccess.Mongo;
using Navtrack.Shared.Library.DI;

namespace Navtrack.DataAccess.Services.Positions;

[Service(typeof(IConnectionRepository))]
public class ConnectionRepository : GenericRepository<DeviceConnectionDocument>, IConnectionRepository
{
    public ConnectionRepository(IRepository repository) : base(repository)
    {
    }

    public Task AddMessage(ObjectId connectionId, byte[] hex)
    {
        return repository.GetCollection<DeviceConnectionDocument>()
            .UpdateOneAsync(x => x.Id == connectionId,
                Builders<DeviceConnectionDocument>.Update.Push(x => x.Messages, hex));
    }
}