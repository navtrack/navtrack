using Navtrack.Api.Model.Protocols;
using Protocol = Navtrack.DataAccess.Model.Protocols.Protocol;

namespace Navtrack.Api.Services.Mappers;

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