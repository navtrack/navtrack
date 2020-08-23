using System.Threading.Tasks;
using Navtrack.DataAccess.Model;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Services
{
    public interface IConnectionService
    {
        Task<DeviceConnectionEntity> NewConnection(string endPoint, int protocolPort);
        Task MarkConnectionAsClosed(DeviceConnectionEntity deviceConnection);
        Task AddMessage(int deviceConnectionId, string hex);
        Task SetDeviceId(Client client);
    }
}