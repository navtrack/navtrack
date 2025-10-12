using System.Net;
using System.Threading.Tasks;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Teams;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.Mappers;
using Navtrack.Api.Services.Organizations.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Teams;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Teams;

[Service(typeof(IRequestHandler<GetTeamAssetsRequest, ListModel<TeamAssetModel>>))]
public class GetTeamAssetsRequestHandler(
    ITeamRepository teamRepository,
    IAssetRepository assetRepository) : BaseRequestHandler<GetTeamAssetsRequest, ListModel<TeamAssetModel>>
{
    private TeamEntity? team;

    public override async Task Validate(RequestValidationContext<GetTeamAssetsRequest> context)
    {
        team = await teamRepository.GetById(context.Request.TeamId);
        team.ThrowApiExceptionIfNull(HttpStatusCode.NotFound);
    }

    public override async Task<ListModel<TeamAssetModel>> Handle(GetTeamAssetsRequest request)
    {
        System.Collections.Generic.List<TeamAssetEntity> assets = await assetRepository.GetByTeamId(team!.Id);
        
        ListModel<TeamAssetModel> result = ListMapper.Map(assets, TeamAssetMapper.Map);

        return result;
    }
}