using System.Threading.Tasks;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Services.Organizations.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Services.Organizations;
using Navtrack.Database.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Organizations;

[Service(typeof(IRequestHandler<CreateOrganizationRequest, Entity>))]
public class CreateOrganizationRequestHandler(
    IOrganizationRepository organizationRepository,
    IUserRepository userRepository) : BaseRequestHandler<CreateOrganizationRequest, Entity>
{
    public override async Task<Entity> Handle(CreateOrganizationRequest request)
    {
        OrganizationEntity organization =
            OrganizationEntityMapper.Map(request.Model.Name, request.OwnerId);
        await organizationRepository.Add(organization);

        OrganizationUserEntity userOrganization = OrganizationUserEntityMapper.Map(
            organization.Id,
            request.OwnerId,
            OrganizationUserRole.Owner,
            request.OwnerId
        );

        await userRepository.AddUserToOrganization(userOrganization);
        await organizationRepository.UpdateUsersCount(organization.Id);

        return new Entity(organization.Id.ToString());
    }
}