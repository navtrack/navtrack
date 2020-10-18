using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Devices;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.DataAccess.Services;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Devices
{
    [Service(typeof(IDeviceTypeService))]
    public class DeviceTypeService : IDeviceTypeService
    {
        private readonly IDeviceTypeDataService deviceTypeDataService;
        private readonly IMapper mapper;

        public DeviceTypeService(IDeviceTypeDataService deviceTypeDataService, IMapper mapper)
        {
            this.deviceTypeDataService = deviceTypeDataService;
            this.mapper = mapper;
        }

        public List<DeviceTypeResponseModel> GetDeviceTypes()
        {
            List<DeviceTypeResponseModel> devicesTypes = deviceTypeDataService.GetDeviceTypes()
                .Select(mapper.Map<DeviceType, DeviceTypeResponseModel>)
                .OrderBy(x => x.DisplayName)
                .ToList();

            return devicesTypes;
        }
    }
}