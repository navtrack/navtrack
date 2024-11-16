using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Organizations;
using Navtrack.Api.Services.Common.Context;
using Navtrack.Api.Services.Common.Mappers;
using Navtrack.Api.Services.Organizations.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Organizations;
using Navtrack.DataAccess.Services.Organizations;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Organizations;

[Service(typeof(IRequestHandler<GetOrganizationsRequest, List<Organization>>))]
public class GetOrganizationsRequestHandler(
    IOrganizationRepository organizationRepository,
    INavtrackContextAccessor navtrackContextAccessor)
    : BaseRequestHandler<GetOrganizationsRequest, List<Organization>>
{
    public override async Task<List<Organization>> Handle(GetOrganizationsRequest request)
    {
        System.Collections.Generic.List<ObjectId> organizationIds = navtrackContextAccessor.NavtrackContext.User
            .Organizations?.Select(x => x.OrganizationId)
            .ToList() ?? [];

        System.Collections.Generic.List<OrganizationDocument> organizations = await organizationRepository.GetByIds(organizationIds);

        List<Organization> model = ListMapper.Map(organizations, source => OrganizationMapper.Map(source));

        return model;
    }
}