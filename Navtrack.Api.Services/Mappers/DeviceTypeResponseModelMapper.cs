using Navtrack.Api.Model.Devices;
using Navtrack.Api.Model.Protocols;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Mappers
{
    [Service(typeof(IMapper<DeviceType, DeviceTypeModel>))]
    public class DeviceTypeResponseModelMapper : IMapper<DeviceType, DeviceTypeModel>
    {
        private readonly IMapper mapper;

        public DeviceTypeResponseModelMapper(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public DeviceTypeModel Map(DeviceType source, DeviceTypeModel destination)
        {
            destination.Id = source.Id;
            destination.DisplayName = $"{source.Manufacturer} {source.Type}";
            destination.Manufacturer = source.Manufacturer;
            destination.Model = source.Type;
            destination.Protocol = mapper.Map<Protocol, ProtocolModel>(source.Protocol);

            return destination;
        }
    }
}