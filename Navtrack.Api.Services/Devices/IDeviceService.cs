using System.Threading.Tasks;

namespace Navtrack.Api.Services.Devices
{
    public interface IDeviceService
    {
        Task<bool> DeviceIsUsed(string deviceId, int deviceTypeId, int? excludeAssetId = null);
    }
}