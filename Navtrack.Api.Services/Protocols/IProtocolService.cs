using Navtrack.Api.Model.Protocols;

namespace Navtrack.Api.Services.Protocols;

public interface IProtocolService
{
    ProtocolsModel GetProtocols();
}