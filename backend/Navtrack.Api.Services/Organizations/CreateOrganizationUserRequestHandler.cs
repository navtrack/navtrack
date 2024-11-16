using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Common.Context;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Organizations.Events;
using Navtrack.Api.Services.Organizations.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Organizations;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Organizations;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;
using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Organizations;

[Service(typeof(IRequestHandler<CreateOrganizationUserRequest>))]
public class CreateOrganizationUserRequestHandler(
    IOrganizationRepository organizationRepository,
    IUserRepository userRepository,
    INavtrackContextAccessor navtrackContextAccessor)
    : BaseRequestHandler<CreateOrganizationUserRequest>
{
    private OrganizationDocument? organization;
    private UserDocument? user;

    public override async Task Validate(RequestValidationContext<CreateOrganizationUserRequest> context)
    {
        organization = await organizationRepository.GetById(context.Request.OrganizationId);
        organization.Return404IfNull();

        user = await userRepository.GetByEmail(context.Request.Model.Email);
        context.ValidationException.AddErrorIfNull(user, nameof(context.Request.Model.Email),
            ApiErrorCodes.User_000001_EmailNotFound);
        
        context.ValidationException.AddErrorIfTrue(
            user?.Organizations?.Any(x => x.OrganizationId == organization.Id),
            nameof(context.Request.Model.Email), ApiErrorCodes.Organization_000001_UserAlreadyInOrganization);
    }

    public override async Task Handle(CreateOrganizationUserRequest request)
    {
        UserOrganizationElement element = UserOrganizationElementMapper.Map(organization!.Id, request.Model.UserRole,
            navtrackContextAccessor.NavtrackContext.User.Id);

        await userRepository.AddUserToOrganization(user!.Id, element);
        await organizationRepository.UpdateUsersCount(organization.Id);
    }

    public override IEvent GetEvent(CreateOrganizationUserRequest request)
    {
        return new UserOrganizationCreatedEvent
        {
            UserId = user!.Id.ToString(),
            OrganizationId = organization!.Id.ToString()
        };
    }
}