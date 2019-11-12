using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Navtrack.Web.Model.Models;

namespace Navtrack.Web.Mappers
{
    [Service(typeof(IMapper<Device, DeviceModel>))]
    public class DeviceMapper : IMapper<Device, DeviceModel>
    {
        public DeviceModel Map(Device source, DeviceModel destination)
        {
            destination.Id = source.Id;
            destination.IMEI = source.IMEI;

            return destination;
        }
    }
}