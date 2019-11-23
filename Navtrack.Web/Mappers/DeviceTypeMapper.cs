using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Navtrack.Web.Model.Models;

namespace Navtrack.Web.Mappers
{
    [Service(typeof(IMapper<DeviceType, DeviceTypeModel>))]
    public class DeviceTypeMapper : IMapper<DeviceType, DeviceTypeModel>
    {
        public DeviceTypeModel Map(DeviceType source, DeviceTypeModel destination)
        {
            destination.Id = source.Id;
            destination.Name = $"{source.Brand} {source.Model}";
            destination.ProtocolId = source.ProtocolId;

            return destination;
        }
    }
}