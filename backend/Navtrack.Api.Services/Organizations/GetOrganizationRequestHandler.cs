using System.Threading.Tasks;
using Navtrack.Api.Model.Organizations;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Organizations.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Services.Organizations;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Organizations;

[Service(typeof(IRequestHandler<GetOrganizationRequest, OrganizationModel>))]
public class GetOrganizationRequestHandler(
    IOrganizationRepository organizationRepository) : BaseRequestHandler<GetOrganizationRequest, OrganizationModel>
{
    public override async Task<OrganizationModel> Handle(GetOrganizationRequest request)
    {
        OrganizationEntity? organization = await organizationRepository.GetById(request.OrganizationId);
        organization.Return404IfNull();
        
        OrganizationModel result = OrganizationModelMapper.Map(organization);
        
        return result;
    }
}
