using Navtrack.Api.Model.Protocols;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Mappers
{
    [Service(typeof(IMapper<Protocol, ProtocolModel>))]
    public class ProtocolResponseModelMapper : IMapper<Protocol, ProtocolModel>
    {
        public ProtocolModel Map(Protocol source, ProtocolModel destination)
        {
            destination.Name = source.Name;
            destination.Port = source.Port;

            return destination;
        }
    }
}