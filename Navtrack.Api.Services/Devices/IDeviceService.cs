using System.Threading.Tasks;
using Navtrack.Api.Model.Devices;

namespace Navtrack.Api.Services.Devices;

public interface IDeviceService
{
    Task<bool> SerialNumberIsUsed(string serialNumber, string deviceTypeId, string? excludeAssetId = null);
    Task<DevicesModel> Get(string assetId);
    Task Add(string assetId, AddDeviceModel model);
    Task Delete(string id);
}