using System.Collections.Generic;
using Navtrack.Api.Model.Protocols;
using Navtrack.Api.Services.Mappers;
using Navtrack.DataAccess.Model.Protocols;
using Navtrack.DataAccess.Services.Protocols;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.Protocols;

[Service(typeof(IProtocolService))]
public class ProtocolService : IProtocolService
{
    private readonly IProtocolDataService protocolDataService;

    public ProtocolService(IProtocolDataService protocolDataService)
    {
        this.protocolDataService = protocolDataService;
    }

    public ProtocolsModel GetProtocols()
    {
        IEnumerable<Protocol> protocols = protocolDataService.GetProtocols();

        return ProtocolListMapper.Map(protocols);
    }
}