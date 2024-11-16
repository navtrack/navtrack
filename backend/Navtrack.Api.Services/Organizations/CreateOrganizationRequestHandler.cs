using System.Threading.Tasks;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Services.Organizations.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Organizations;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Organizations;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Organizations;

[Service(typeof(IRequestHandler<CreateOrganizationRequest, Entity>))]
public class CreateOrganizationRequestHandler(
    IOrganizationRepository organizationRepository,
    IUserRepository userRepository) : BaseRequestHandler<CreateOrganizationRequest, Entity>
{
    public override async Task<Entity> Handle(CreateOrganizationRequest request)
    {
        OrganizationDocument organization =
            OrganizationDocumentMapper.Map(request.Model.Name, request.OwnerId);
        await organizationRepository.Add(organization);

        UserOrganizationElement userOrganization = UserOrganizationElementMapper.Map(
            organization.Id,
            OrganizationUserRole.Owner,
            request.OwnerId
        );

        await userRepository.AddUserToOrganization(request.OwnerId, userOrganization);
        await organizationRepository.UpdateUsersCount(organization.Id);

        return new Entity(organization.Id.ToString());
    }
}