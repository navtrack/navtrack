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
    private OrganizationEntity? organization;
    
    public override async Task Validate(RequestValidationContext<GetOrganizationRequest> context)
    {
        organization = await organizationRepository.GetById(context.Request.OrganizationId);
        organization.Return404IfNull();
    }
    
    public override Task<OrganizationModel> Handle(GetOrganizationRequest request)
    {
        OrganizationModel result = OrganizationModelMapper.Map(organization!);
        
        return Task.FromResult(result);
    }
}