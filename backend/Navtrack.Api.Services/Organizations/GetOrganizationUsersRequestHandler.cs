using System.Threading.Tasks;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Organizations;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.Mappers;
using Navtrack.Api.Services.Organizations.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Services.Organizations;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Organizations;

[Service(typeof(IRequestHandler<GetOrganizationUsersRequest, ListModel<OrganizationUserModel>>))]
public class GetOrganizationUsersRequestHandler(IOrganizationRepository organizationRepository)
    : BaseRequestHandler<GetOrganizationUsersRequest, ListModel<OrganizationUserModel>>
{
    public override async Task<ListModel<OrganizationUserModel>> Handle(GetOrganizationUsersRequest request)
    {
        OrganizationEntity? organization = await organizationRepository.GetById(request.OrganizationId);
        organization.Return404IfNull();
        
        System.Collections.Generic.List<OrganizationUserEntity> organizationUsers =
            await organizationRepository.GetUsers(organization.Id);

        ListModel<OrganizationUserModel> result = ListMapper.Map(organizationUsers, OrganizationUserModelMapper.Map);

        return result;
    }
}
