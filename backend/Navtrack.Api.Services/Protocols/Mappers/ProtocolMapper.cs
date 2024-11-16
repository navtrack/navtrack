using Protocol = Navtrack.Api.Model.Protocols.Protocol;

namespace Navtrack.Api.Services.Protocols.Mappers;

public static class ProtocolMapper
{
    public static Protocol Map(DataAccess.Model.Protocols.Protocol source)
    {
        return new Protocol
        {
            Name = source.Name,
            Port = source.Port
        };
    }
}