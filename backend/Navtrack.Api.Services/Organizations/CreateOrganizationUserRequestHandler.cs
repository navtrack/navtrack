using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.RequestContext;
using Navtrack.Api.Services.Organizations.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Services.Organizations;
using Navtrack.Database.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Organizations;

[Service(typeof(IRequestHandler<CreateOrganizationUserRequest>))]
public class CreateOrganizationUserRequestHandler(
    IOrganizationRepository organizationRepository,
    IUserRepository userRepository,
    INavtrackRequestContextAccessor navtrackRequestContextAccessor)
    : BaseRequestHandler<CreateOrganizationUserRequest>
{
    public override async Task Handle(CreateOrganizationUserRequest request)
    {
        OrganizationEntity? organization = await organizationRepository.GetById(request.OrganizationId);
        organization.Return404IfNull();

        UserEntity? user = await userRepository.GetByEmail(request.Model.Email);
        ValidationApiException validationException = new();
        
        validationException.AddErrorIfNull(user, nameof(request.Model.Email), ApiErrorCodes.User_EmailNotFound);
        validationException.AddErrorIfTrue(
            user?.OrganizationUsers.Any(x => x.OrganizationId == organization.Id),
            nameof(request.Model.Email), ApiErrorCodes.Organization_UserAlreadyInOrganization);
        validationException.ThrowIfInvalid();
        
        OrganizationUserEntity element = OrganizationUserEntityMapper.Map(organization.Id, user.Id, request.Model.UserRole,
            navtrackRequestContextAccessor.NavtrackContext.CurrentUser.Id);

        await userRepository.AddUserToOrganization(element);
        await organizationRepository.UpdateUsersCount(organization.Id);
    }
}
