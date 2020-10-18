using Navtrack.Api.Model.Devices;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.DataAccess.Services;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Mappers
{
    [Service(typeof(IMapper<DeviceEntity, DeviceResponseModel>))]
    [Service(typeof(IMapper<DeviceResponseModel, DeviceEntity>))]
    public class DeviceMapper : IMapper<DeviceEntity, DeviceResponseModel>, IMapper<DeviceResponseModel, DeviceEntity>
    {
        private readonly IDeviceTypeDataService deviceTypeDataService;
        private readonly IMapper mapper;

        public DeviceMapper(IDeviceTypeDataService deviceTypeDataService, IMapper mapper)
        {
            this.deviceTypeDataService = deviceTypeDataService;
            this.mapper = mapper;
        }

        public DeviceResponseModel Map(DeviceEntity source, DeviceResponseModel destination)
        {
            destination.Id = source.Id;
            destination.DeviceId = source.DeviceId;
            destination.IsActive = source.IsActive;
            destination.AssetId = source.AssetId;
            
            DeviceType deviceType = deviceTypeDataService.GetById(source.DeviceTypeId);
            destination.DeviceType = deviceType != null
                ? mapper.Map<DeviceType, DeviceTypeResponseModel>(deviceType)
                : null;

            return destination;
        }

        public DeviceEntity Map(DeviceResponseModel source, DeviceEntity destination)
        {
            destination.Id = source.Id;
            destination.DeviceId = source.DeviceId;

            return destination;
        }
    }
}