using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Navtrack.Web.Model.Models;

namespace Navtrack.Web.Mappers
{
    [Service(typeof(IMapper<Device, DeviceModel>))]
    [Service(typeof(IMapper<DeviceModel, Device>))]
    public class DeviceMapper : IMapper<Device, DeviceModel>, IMapper<DeviceModel, Device>
    {
        public DeviceModel Map(Device source, DeviceModel destination)
        {
            destination.Id = source.Id;
            destination.IMEI = source.IMEI;
            destination.Name = source.Name;

            return destination;
        }

        public Device Map(DeviceModel source, Device destination)
        {
            destination.Id = source.Id;
            destination.IMEI = source.IMEI;
            destination.Name = source.Name;

            return destination;
        }
    }
}