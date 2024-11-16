using System.Threading.Tasks;
using Navtrack.Api.Model.Organizations;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Organizations.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Organizations;
using Navtrack.DataAccess.Services.Organizations;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Organizations;

[Service(typeof(IRequestHandler<GetOrganizationRequest, Organization>))]
public class GetOrganizationRequestHandler(
    IOrganizationRepository organizationRepository) : BaseRequestHandler<GetOrganizationRequest, Organization>
{
    private OrganizationDocument? organization;
    public override async Task Validate(RequestValidationContext<GetOrganizationRequest> context)
    {
        organization = await organizationRepository.GetById(context.Request.OrganizationId);
        organization.Return404IfNull();
    }
    
    public override Task<Organization> Handle(GetOrganizationRequest request)
    {
        Organization result = OrganizationMapper.Map(organization!);
        
        return Task.FromResult(result);
    }
}