using Navtrack.Api.Models;
using Navtrack.DeviceData.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Mappers
{
    [Service(typeof(IMapper<Protocol, ProtocolModel>))]
    public class ProtocolMapper : IMapper<Protocol, ProtocolModel>
    {
        public ProtocolModel Map(Protocol source, ProtocolModel destination)
        {
            destination.Name = source.Name;
            destination.Port = source.Port;

            return destination;
        }
    }
}