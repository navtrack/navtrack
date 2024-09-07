using Navtrack.Api.Model.Protocols;
using Navtrack.DataAccess.Model.Protocols;

namespace Navtrack.Api.Services.Protocols.Mappers;

public static class ProtocolModelMapper
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