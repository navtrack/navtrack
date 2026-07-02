using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Services.Organizations;
using Navtrack.Database.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Organizations;

[Service(typeof(IRequestHandler<DeleteOrganizationUserRequest>))]
public class DeleteOrganizationUserRequestHandler(
    IOrganizationRepository organizationRepository,
    IUserRepository userRepository)
    : BaseRequestHandler<DeleteOrganizationUserRequest>
{
    public override async Task Handle(DeleteOrganizationUserRequest request)
    {
        OrganizationEntity? organization = await organizationRepository.GetById(request.OrganizationId);
        organization.Return404IfNull();

        UserEntity? user = await userRepository.GetById(request.UserId);
        user.Return404IfNull();

        OrganizationUserEntity? userOrganization = user.OrganizationUsers.FirstOrDefault(x => x.OrganizationId == organization.Id);
        userOrganization.Return404IfNull();

        int ownersCount = await userRepository.GetOrganizationOwnersCount(organization.Id);

        if (ownersCount == 1 && userOrganization.UserRole == OrganizationUserRole.Owner)
        {
            throw new ApiException(ApiErrorCodes.Organization_OneOwnerRequired);
        }
        
        await userRepository.DeleteUserFromOrganization(user.Id, organization.Id);
        await organizationRepository.UpdateUsersCount(organization.Id);
    }
}
