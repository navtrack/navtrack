using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Models;
using Navtrack.DeviceData.Services;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using DeviceModel = Navtrack.DeviceData.Model.DeviceModel;

namespace Navtrack.Api.Services
{
    [Service(typeof(IDeviceModelService))]
    public class DeviceModelService : IDeviceModelService
    {
        private readonly IDeviceModelDataService deviceModelDataService;
        private readonly IMapper mapper;

        public DeviceModelService(IDeviceModelDataService deviceModelDataService, IMapper mapper)
        {
            this.deviceModelDataService = deviceModelDataService;
            this.mapper = mapper;
        }

        public List<DeviceModelModel> GetModels()
        {
            List<DeviceModelModel> devicesTypes = deviceModelDataService.GetDeviceModels()
                .Select(mapper.Map<DeviceModel, DeviceModelModel>)
                .ToList();

            return devicesTypes;
        }
    }
}