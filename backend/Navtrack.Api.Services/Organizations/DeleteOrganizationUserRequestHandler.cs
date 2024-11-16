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

[Service(typeof(IRequestHandler<DeleteOrganizationUserRequest>))]
public class DeleteOrganizationUserRequestHandler(
    IOrganizationRepository organizationRepository,
    IUserRepository userRepository)
    : BaseRequestHandler<DeleteOrganizationUserRequest>
{
    private OrganizationDocument? organization;
    private UserDocument? user;

    public override async Task Validate(RequestValidationContext<DeleteOrganizationUserRequest> context)
    {
        organization = await organizationRepository.GetById(context.Request.OrganizationId);
        organization.Return404IfNull();

        user = await userRepository.GetById(context.Request.UserId);
        user.Return404IfNull();

        UserOrganizationElement? userOrganization =
            user.Organizations?.FirstOrDefault(x => x.OrganizationId == organization.Id);
        userOrganization.Return404IfNull();

        int ownersCount = await userRepository.GetOrganizationOwnersCount(organization.Id);

        if (ownersCount == 1 && userOrganization.UserRole == OrganizationUserRole.Owner)
        {
            throw new ApiException(ApiErrorCodes.Organization_000002_OneOwnerRequired);
        }
    }

    public override async Task Handle(DeleteOrganizationUserRequest request)
    {
        await userRepository.DeleteUserFromOrganization(user!.Id, organization!.Id);
        await organizationRepository.UpdateUsersCount(organization.Id);
    }

    public override IEvent GetEvent(DeleteOrganizationUserRequest request)
    {
        return new UserOrganizationDeletedEvent
        {
            UserId = user!.Id.ToString(),
            OrganizationId = organization!.Id.ToString()
        };
    }
}