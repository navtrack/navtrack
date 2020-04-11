using Navtrack.Api.Models;
using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Mappers
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