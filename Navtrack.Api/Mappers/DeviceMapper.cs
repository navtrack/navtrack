using Navtrack.Api.Models;
using Navtrack.DataAccess.Model;
using Navtrack.DeviceData.Services;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Mappers
{
    [Service(typeof(IMapper<DeviceEntity, DeviceModel>))]
    [Service(typeof(IMapper<DeviceModel, DeviceEntity>))]
    public class DeviceMapper : IMapper<DeviceEntity, DeviceModel>, IMapper<DeviceModel, DeviceEntity>
    {
        private readonly IDeviceModelDataService deviceModelDataService;
        private readonly IMapper mapper;

        public DeviceMapper(IDeviceModelDataService deviceModelDataService, IMapper mapper)
        {
            this.deviceModelDataService = deviceModelDataService;
            this.mapper = mapper;
        }

        public DeviceModel Map(DeviceEntity source, DeviceModel destination)
        {
            destination.Id = source.Id;
            destination.DeviceId = source.DeviceId;
            destination.Name = source.Name;
            destination.DeviceModelId = source.DeviceModelId;
            
            DeviceData.Model.DeviceModel deviceModel = deviceModelDataService.GetById(source.DeviceModelId);
            destination.Model = deviceModel != null
                ? mapper.Map<DeviceData.Model.DeviceModel, DeviceModelModel>(deviceModel)
                : null;

            return destination;
        }

        public DeviceEntity Map(DeviceModel source, DeviceEntity destination)
        {
            destination.Id = source.Id;
            destination.DeviceId = source.DeviceId;
            destination.Name = source.Name;
            destination.DeviceModelId = source.DeviceModelId;

            return destination;
        }
    }
}