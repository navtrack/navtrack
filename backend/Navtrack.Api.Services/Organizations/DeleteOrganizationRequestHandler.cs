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
    private OrganizationEntity? organization;

    public override async Task Validate(RequestValidationContext<DeleteOrganizationRequest> context)
    {
        organization = await organizationRepository.GetById(context.Request.OrganizationId);
        organization.Return404IfNull();
    }

    public override Task Handle(DeleteOrganizationRequest request)
    {
        return organizationRepository.Delete(organization!);
    }
}