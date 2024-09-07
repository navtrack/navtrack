using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Devices.Connections;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Services.Devices;

public interface IDeviceConnectionRepository : IGenericRepository<DeviceConnectionDocument>
{
    Task AddMessage(ObjectId connectionId, byte[] hex);
}