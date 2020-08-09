using Navtrack.Api.Model.Devices;
using Navtrack.Api.Model.Protocols;
using Navtrack.DeviceData.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Mappers
{
    [Service(typeof(IMapper<DeviceType, DeviceTypeResponseModel>))]
    public class DeviceTypeResponseModelMapper : IMapper<DeviceType, DeviceTypeResponseModel>
    {
        private readonly IMapper mapper;

        public DeviceTypeResponseModelMapper(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public DeviceTypeResponseModel Map(DeviceType source, DeviceTypeResponseModel destination)
        {
            destination.Id = source.Id;
            destination.DisplayName = $"{source.Manufacturer} {source.Type}";
            destination.Manufacturer = source.Manufacturer;
            destination.Model = source.Type;
            destination.Protocol = mapper.Map<Protocol, ProtocolResponseModel>(source.Protocol);

            return destination;
        }
    }
}