using System.Threading.Tasks;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Devices;

namespace Navtrack.Api.Services.Devices;

public interface IDeviceService
{
    Task<bool> SerialNumberIsUsed(string serialNumber, string deviceTypeId, string? excludeAssetId = null);
    Task<ListModel<DeviceModel>> Get(string assetId);
    Task Change(string assetId, ChangeDeviceModel model);
    Task Delete(string assetId, string deviceId);
}