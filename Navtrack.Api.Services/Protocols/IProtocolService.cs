using Navtrack.Api.Model.Protocols;

namespace Navtrack.Api.Services.Old.Protocols;

public interface IProtocolService
{
    ProtocolListModel GetProtocols();
}