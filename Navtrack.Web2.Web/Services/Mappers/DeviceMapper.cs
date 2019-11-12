using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Navtrack.Web.Models;

namespace Navtrack.Web.Services.Mappers
{
    [Service(typeof(IMapper<DataAccess.Model.Device, Device>))]
    public class DeviceMapper : IMapper<DataAccess.Model.Device, Device>
    {
        public Device Map(DataAccess.Model.Device source, Device destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Object?.Name;
            destination.IMEI = source.IMEI;

            return destination;
        }
    }
}