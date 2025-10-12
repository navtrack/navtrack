using Navtrack.Api.Model.Protocols;
using Navtrack.Database.Model.Protocols;

namespace Navtrack.Api.Services.Protocols.Mappers;

public static class ProtocolMapper
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