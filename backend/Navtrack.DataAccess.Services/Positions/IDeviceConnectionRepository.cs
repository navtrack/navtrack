using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Devices.Connections;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Services.Positions;

public interface IDeviceConnectionRepository : IGenericRepository<DeviceConnectionDocument>
{
    Task AddMessage(ObjectId connectionId, byte[] hex);
}