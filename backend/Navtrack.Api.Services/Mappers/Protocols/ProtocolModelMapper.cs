using Navtrack.Api.Model.Protocols;
using Navtrack.DataAccess.Model.Protocols;

namespace Navtrack.Api.Services.Mappers.Protocols;

public class ProtocolModelMapper
{
    public static ProtocolModel Map(Protocol source)
    {
        return new ProtocolModel
        {
            Name = source.Name,
            Port = source.Port
        };
    }
}