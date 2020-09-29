using System.Threading.Tasks;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.DataAccess.Services;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.Devices
{
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

        public Task<bool> DeviceIsUsed(string deviceId, int deviceTypeId, int? excludeAssetId = null)
        {
            DeviceType deviceType = deviceTypeDataService.GetById(deviceTypeId);

            return deviceDataService.DeviceIsAssigned(deviceId, deviceType.Protocol.Port, excludeAssetId);
        }
    }
}