using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Navtrack.DataAccess.Model.Positions;
using Navtrack.DataAccess.Mongo;
using Navtrack.Shared.Library.DI;

namespace Navtrack.DataAccess.Services.Positions;

[Service(typeof(IConnectionRepository))]
public class ConnectionRepository : GenericRepository<ConnectionDocument>, IConnectionRepository
{
    public ConnectionRepository(IRepository repository) : base(repository)
    {
    }

    public Task AddMessage(ObjectId connectionId, byte[] hex)
    {
        return repository.GetCollection<ConnectionDocument>()
            .UpdateOneAsync(x => x.Id == connectionId,
                Builders<ConnectionDocument>.Update.Push(x => x.Messages, hex));
    }
}