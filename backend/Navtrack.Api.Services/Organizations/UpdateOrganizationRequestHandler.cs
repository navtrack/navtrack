using System.Threading.Tasks;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Services.Organizations;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Organizations;

[Service(typeof(IRequestHandler<UpdateOrganizationRequest>))]
public class UpdateOrganizationRequestHandler(
    IOrganizationRepository organizationRepository) : BaseRequestHandler<UpdateOrganizationRequest>
{
    private OrganizationEntity? organization;

    public override async Task Validate(RequestValidationContext<UpdateOrganizationRequest> context)
    {
        organization = await organizationRepository.GetById(context.Request.OrganizationId);
        organization.Return404IfNull();
    }

    public override async Task Handle(UpdateOrganizationRequest request)
    {
        if (!string.IsNullOrEmpty(request.Model.Name) && organization!.Name != request.Model.Name)
        {
            await organizationRepository.UpdateName(request.OrganizationId, request.Model.Name);
        }
    }
}