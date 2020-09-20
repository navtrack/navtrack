using Navtrack.Api.Model.Devices;
using Navtrack.DataAccess.Model;
using Navtrack.DeviceData.Services;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Mappers
{
    [Service(typeof(IMapper<DeviceEntity, DeviceModel>))]
    [Service(typeof(IMapper<DeviceModel, DeviceEntity>))]
    public class DeviceMapper : IMapper<DeviceEntity, DeviceModel>, IMapper<DeviceModel, DeviceEntity>
    {
        private readonly IDeviceTypeDataService deviceTypeDataService;
        private readonly IMapper mapper;

        public DeviceMapper(IDeviceTypeDataService deviceTypeDataService, IMapper mapper)
        {
            this.deviceTypeDataService = deviceTypeDataService;
            this.mapper = mapper;
        }

        public DeviceModel Map(DeviceEntity source, DeviceModel destination)
        {
            destination.Id = source.Id;
            destination.DeviceId = source.DeviceId;
            destination.IsActive = source.IsActive;
            destination.AssetId = source.AssetId;
            
            DeviceData.Model.DeviceType deviceType = deviceTypeDataService.GetById(source.DeviceTypeId);
            destination.DeviceType = deviceType != null
                ? mapper.Map<DeviceData.Model.DeviceType, DeviceTypeModel>(deviceType)
                : null;

            return destination;
        }

        public DeviceEntity Map(DeviceModel source, DeviceEntity destination)
        {
            destination.Id = source.Id;
            destination.DeviceId = source.DeviceId;

            return destination;
        }
    }
}