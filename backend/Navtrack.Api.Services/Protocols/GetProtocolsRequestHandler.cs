using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Api.Model.Protocols;
using Navtrack.Api.Services.Common.Mappers;
using Navtrack.Api.Services.Protocols.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Services.Protocols;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Protocols;

[Service(typeof(IRequestHandler<GetProtocolsRequest, Model.Common.List<Protocol>>))]
public class GetProtocolsRequestHandler(IProtocolRepository protocolRepository) 
    : BaseRequestHandler<GetProtocolsRequest, Model.Common.List<Protocol>>
{
    public override Task<Model.Common.List<Protocol>> Handle(GetProtocolsRequest request)
    {
        IEnumerable<DataAccess.Model.Protocols.Protocol> protocols = protocolRepository.GetProtocols();

        Model.Common.List<Protocol> result = ListMapper.Map(protocols, ProtocolMapper.Map);
        
        return Task.FromResult(result);
    }
}