using System.Collections.Generic;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Protocols;
using Navtrack.Api.Services.Mappers.Protocols;
using Navtrack.DataAccess.Model.Protocols;
using Navtrack.DataAccess.Services.Protocols;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Protocols;

[Service(typeof(IProtocolService))]
public class ProtocolService : IProtocolService
{
    private readonly IProtocolRepository protocolRepository;

    public ProtocolService(IProtocolRepository protocolRepository)
    {
        this.protocolRepository = protocolRepository;
    }

    public ListModel<ProtocolModel> GetProtocols()
    {
        IEnumerable<Protocol> protocols = protocolRepository.GetProtocols();

        return ProtocolListMapper.Map(protocols);
    }
}