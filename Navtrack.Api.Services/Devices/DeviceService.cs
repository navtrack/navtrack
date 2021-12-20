using System.Threading.Tasks;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.DataAccess.Services.Devices;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.Devices;

[Service(typeof(IDeviceService))]
public class DeviceService : IDeviceService
{
    private readonly IDeviceTypeDataService deviceTypeDataService;
    private readonly IDeviceDataService deviceDataService;

    public DeviceService(IDeviceTypeDataService deviceTypeDataService, IDeviceDataService deviceDataService)
    {
        this.deviceTypeDataService = deviceTypeDataService;
        this.deviceDataService = deviceDataService;
    }

    public Task<bool> SerialNumberIsUsed(string serialNumber, string deviceTypeId, string excludeAssetId = null)
    {
        DeviceType deviceType = deviceTypeDataService.GetById(deviceTypeId);

        return deviceDataService.SerialNumberIsUsed(serialNumber, deviceType.Protocol.Port, excludeAssetId);
    }
}