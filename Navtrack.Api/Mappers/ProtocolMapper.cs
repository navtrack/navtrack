using Navtrack.Api.Models;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Mappers
{
    [Service(typeof(IMapper<Protocol, ProtocolModel>))]
    public class ProtocolMapper : IMapper<Protocol, ProtocolModel>
    {
        public ProtocolModel Map(Protocol source, ProtocolModel destination)
        {
            destination.Id = (int) source;
            destination.Name = source.ToString();

            return destination;
        }
    }
}