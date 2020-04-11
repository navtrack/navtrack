using Navtrack.Api.Models;
using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Mappers
{
    [Service(typeof(IMapper<Device, DeviceModel>))]
    [Service(typeof(IMapper<DeviceModel, Device>))]
    public class DeviceMapper : IMapper<Device, Models.DeviceModel>, IMapper<Models.DeviceModel, Device>
    {
        public DeviceModel Map(Device source, DeviceModel destination)
        {
            destination.Id = source.Id;
            destination.IMEI = source.IMEI;
            destination.Name = source.Name;
            destination.DeviceTypeId = source.DeviceTypeId;
            destination.Type = $"{source.DeviceType.Brand} {source.DeviceType.Model}";

            return destination;
        }

        public Device Map(DeviceModel source, Device destination)
        {
            destination.Id = source.Id;
            destination.IMEI = source.IMEI;
            destination.Name = source.Name;
            destination.DeviceTypeId = source.DeviceTypeId;

            return destination;
        }
    }
}