using System.Threading.Tasks;
using Navtrack.DataAccess.Model;

namespace Navtrack.DataAccess.Services
{
    public interface IDeviceDataService
    {
        Task<bool> DeviceIsAssigned(string deviceId, int protocolPort, int? excludeAssetId);
        Task<DeviceEntity> GetActiveDeviceByDeviceId(string deviceId);
    }
}