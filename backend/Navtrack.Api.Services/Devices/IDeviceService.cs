using System.Threading.Tasks;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Devices;

namespace Navtrack.Api.Services.Devices;

public interface IDeviceService
{
    Task<bool> SerialNumberIsUsed(string serialNumber, string deviceTypeId, string? excludeAssetId = null);
    Task<ListModel<DeviceModel>> GetList(string assetId);
    Task Change(string assetId, UpdateAssetDeviceModel model);
    Task Delete(string assetId, string deviceId);
}