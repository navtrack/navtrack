using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Positions;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Services.Positions;

public interface IConnectionRepository : IGenericRepository<ConnectionDocument>
{
    Task AddMessage(ObjectId connectionId, byte[] hex);
}