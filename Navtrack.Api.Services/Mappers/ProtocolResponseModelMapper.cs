using Navtrack.Api.Model.Protocols;
using Navtrack.DeviceData.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Mappers
{
    [Service(typeof(IMapper<Protocol, ProtocolResponseModel>))]
    public class ProtocolResponseModelMapper : IMapper<Protocol, ProtocolResponseModel>
    {
        public ProtocolResponseModel Map(Protocol source, ProtocolResponseModel destination)
        {
            destination.Name = source.Name;
            destination.Port = source.Port;

            return destination;
        }
    }
}