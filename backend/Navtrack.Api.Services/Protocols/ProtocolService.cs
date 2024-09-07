using System.Collections.Generic;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Protocols;
using Navtrack.Api.Services.Protocols.Mappers;
using Navtrack.DataAccess.Model.Protocols;
using Navtrack.DataAccess.Services.Protocols;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Protocols;

[Service(typeof(IProtocolService))]
public class ProtocolService(IProtocolRepository repository) : IProtocolService
{
    public ListModel<ProtocolModel> GetProtocols()
    {
        IEnumerable<Protocol> protocols = repository.GetProtocols();

        return ProtocolListMapper.Map(protocols);
    }
}