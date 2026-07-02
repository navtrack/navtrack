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

[Service(typeof(IRequestHandler<UpdateOrganizationUserRequest>))]
public class UpdateOrganizationUserRequestHandler(
    IOrganizationRepository organizationRepository,
    IUserRepository userRepository)
    : BaseRequestHandler<UpdateOrganizationUserRequest>
{
    public override async Task Handle(UpdateOrganizationUserRequest request)
    {
        OrganizationEntity? organization = await organizationRepository.GetById(request.OrganizationId);
        organization.Return404IfNull();

        UserEntity? user = await userRepository.GetById(request.UserId);
        user.Return404IfNull();

        OrganizationUserEntity? userOrganization = user.OrganizationUsers.FirstOrDefault(x => x.OrganizationId == organization.Id);
        userOrganization.Return404IfNull();

        int ownersCount = await userRepository.GetOrganizationOwnersCount(organization.Id);

        if (ownersCount == 1 && userOrganization.UserRole == OrganizationUserRole.Owner &&
            request.Model.UserRole != OrganizationUserRole.Owner)
        {
            throw new ValidationApiException().AddValidationError(nameof(request.Model.UserRole),
                ApiErrorCodes.Organization_OneOwnerRequired);
        }
        
        if (userOrganization.UserRole != request.Model.UserRole)
        {
            await userRepository.UpdateOrganizationUser(user.Id, userOrganization.OrganizationId,
                request.Model.UserRole);
        }
    }
}
