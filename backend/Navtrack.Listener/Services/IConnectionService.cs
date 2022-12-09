using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Services;

public interface IConnectionService
{
    Task<DeviceConnectionDocument> NewConnection(string endPoint, int protocolPort);
    Task MarkConnectionAsClosed(DeviceConnectionDocument deviceConnection);
    Task<ObjectId> AddMessage(ObjectId deviceConnectionId, string hex);
    Task SetDeviceId(Client client);
}