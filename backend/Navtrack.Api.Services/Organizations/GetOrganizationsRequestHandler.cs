using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Organizations;
using Navtrack.Api.Services.Common.Context;
using Navtrack.Api.Services.Common.Mappers;
using Navtrack.Api.Services.Organizations.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Services.Organizations;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Organizations;

[Service(typeof(IRequestHandler<GetOrganizationsRequest, ListModel<OrganizationModel>>))]
public class GetOrganizationsRequestHandler(
    IOrganizationRepository organizationRepository,
    INavtrackContextAccessor navtrackContextAccessor)
    : BaseRequestHandler<GetOrganizationsRequest, ListModel<OrganizationModel>>
{
    public override async Task<ListModel<OrganizationModel>> Handle(GetOrganizationsRequest request)
    {
        List<Guid> organizationIds = navtrackContextAccessor.NavtrackContext.User
            .OrganizationUsers.Select(x => x.OrganizationId)
            .ToList();

        List<OrganizationEntity> organizations = await organizationRepository.GetByIds(organizationIds);

        ListModel<OrganizationModel> model = ListMapper.Map(organizations, source => OrganizationModelMapper.Map(source));

        return model;
    }
}