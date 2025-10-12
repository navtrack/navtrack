using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Api.Model.Protocols;
using Navtrack.Api.Services.Common.Mappers;
using Navtrack.Api.Services.Protocols.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Protocols;
using Navtrack.Database.Services.Protocols;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Protocols;

[Service(typeof(IRequestHandler<GetProtocolsRequest, Model.Common.ListModel<ProtocolModel>>))]
public class GetProtocolsRequestHandler(IProtocolRepository protocolRepository) 
    : BaseRequestHandler<GetProtocolsRequest, Model.Common.ListModel<ProtocolModel>>
{
    public override Task<Model.Common.ListModel<ProtocolModel>> Handle(GetProtocolsRequest request)
    {
        IEnumerable<Protocol> protocols = protocolRepository.GetProtocols();

        Model.Common.ListModel<ProtocolModel> result = ListMapper.Map(protocols, ProtocolMapper.Map);
        
        return Task.FromResult(result);
    }
}