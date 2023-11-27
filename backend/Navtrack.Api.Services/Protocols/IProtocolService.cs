using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Protocols;

namespace Navtrack.Api.Services.Protocols;

public interface IProtocolService
{
    ListModel<ProtocolModel> GetProtocols();
}