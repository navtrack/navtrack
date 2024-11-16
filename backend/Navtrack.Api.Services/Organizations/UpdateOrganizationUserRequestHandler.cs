using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Organizations.Events;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Organizations;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Organizations;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;
using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Organizations;

[Service(typeof(IRequestHandler<UpdateOrganizationUserRequest>))]
public class UpdateOrganizationUserRequestHandler(
    IOrganizationRepository organizationRepository,
    IUserRepository userRepository)
    : BaseRequestHandler<UpdateOrganizationUserRequest>
{
    private OrganizationDocument? organization;
    private UserDocument? user;
    private UserOrganizationElement? userOrganization;

    public override async Task Validate(RequestValidationContext<UpdateOrganizationUserRequest> context)
    {
        organization = await organizationRepository.GetById(context.Request.OrganizationId);
        organization.Return404IfNull();

        user = await userRepository.GetById(context.Request.UserId);
        user.Return404IfNull();

        userOrganization =
            user.Organizations?.FirstOrDefault(x => x.OrganizationId == organization.Id);
        userOrganization.Return404IfNull();

        int ownersCount = await userRepository.GetOrganizationOwnersCount(organization.Id);

        if (ownersCount == 1 && userOrganization.UserRole == OrganizationUserRole.Owner &&
            context.Request.Model.UserRole != OrganizationUserRole.Owner)
        {
            throw new ValidationApiException().AddValidationError(nameof(context.Request.Model.UserRole),
                ApiErrorCodes.Organization_000002_OneOwnerRequired);
        }
    }

    public override async Task Handle(UpdateOrganizationUserRequest request)
    {
        if (userOrganization!.UserRole != request.Model.UserRole)
        {
            await userRepository.UpdateOrganizationUser(user!.Id, userOrganization.OrganizationId,
                request.Model.UserRole);
        }
    }

    public override IEvent GetEvent(UpdateOrganizationUserRequest request)
    {
        return new UserOrganizationUpdatedEvent
        {
            UserId = user!.Id.ToString(),
            OrganizationId = userOrganization!.OrganizationId.ToString()
        };
    }
}