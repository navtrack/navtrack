using System.Threading.Tasks;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Organizations;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Organizations.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Organizations;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Organizations;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Organizations;

[Service(typeof(IRequestHandler<GetOrganizationUsersRequest, List<OrganizationUser>>))]
public class GetOrganizationUsersRequestHandler(
    IOrganizationRepository organizationRepository,
    IUserRepository userRepository) : BaseRequestHandler<GetOrganizationUsersRequest, List<OrganizationUser>>
{
    private OrganizationDocument? organization;

    public override async Task Validate(RequestValidationContext<GetOrganizationUsersRequest> context)
    {
        organization = await organizationRepository.GetById(context.Request.OrganizationId);
        organization.Return404IfNull();
    }

    public override async Task<List<OrganizationUser>> Handle(GetOrganizationUsersRequest request)
    {
        System.Collections.Generic.List<UserDocument>
            users = await userRepository.GetByOrganizationId(organization!.Id);

        List<OrganizationUser> result = OrganizationUserListMapper.Map(users, organization.Id);

        return result;
    }
}