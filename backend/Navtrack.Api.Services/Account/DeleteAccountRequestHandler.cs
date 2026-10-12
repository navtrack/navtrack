using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.Passwords;
using Navtrack.Api.Services.Common.RequestContext;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Account;

[Service(typeof(IRequestHandler<DeleteAccountRequest>))]
public class DeleteAccountRequestHandler(
    IUserRepository userRepository,
    INavtrackRequestContextAccessor navtrackRequestContextAccessor,
    IPasswordHasher passwordHasher)
    : BaseRequestHandler<DeleteAccountRequest>
{
    public override async Task Handle(DeleteAccountRequest request)
    {
        UserEntity? currentUser = navtrackRequestContextAccessor.NavtrackContext?.CurrentUser;
        currentUser.ThrowApiExceptionIfNull(HttpStatusCode.Unauthorized);

        ValidationApiException apiException = new();

        if (!passwordHasher.CheckPassword(request.Model.Password,
                currentUser.PasswordHash,
                currentUser.PasswordSalt))
        {
            apiException.AddValidationError(nameof(request.Model.Password),
                ApiErrorCodes.User_InvalidPassword);
        }

        apiException.ThrowIfInvalid();

        List<OrganizationUserEntity> ownedOrganizations = currentUser.OrganizationUsers
            .Where(x => x.UserRole == OrganizationUserRole.Owner)
            .ToList();

        foreach (OrganizationUserEntity orgUser in ownedOrganizations)
        {
            int ownersCount = await userRepository.GetOrganizationOwnersCount(orgUser.OrganizationId);

            if (ownersCount == 1)
            {
                throw new ApiException(ApiErrorCodes.User_SoleOrganizationOwner);
            }
        }

        await userRepository.Delete(currentUser);
    }
}
