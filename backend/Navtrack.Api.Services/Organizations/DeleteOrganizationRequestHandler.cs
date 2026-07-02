using System.Threading.Tasks;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Services.Organizations;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Organizations;

[Service(typeof(IRequestHandler<DeleteOrganizationRequest>))]
public class DeleteOrganizationRequestHandler(
    IOrganizationRepository organizationRepository) : BaseRequestHandler<DeleteOrganizationRequest>
{
    public override async Task Handle(DeleteOrganizationRequest request)
    {
        OrganizationEntity? organization = await organizationRepository.GetById(request.OrganizationId);
        organization.Return404IfNull();
        
        await organizationRepository.Delete(organization);
    }
}
