using System.Net;
using System.Threading.Tasks;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Teams;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.Mappers;
using Navtrack.Api.Services.Organizations.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Teams;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.DataAccess.Services.Teams;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Teams;

[Service(typeof(IRequestHandler<GetTeamAssetsRequest, List<TeamAsset>>))]
public class GetTeamAssetsRequestHandler(
    ITeamRepository teamRepository,
    IAssetRepository assetRepository) : BaseRequestHandler<GetTeamAssetsRequest, List<TeamAsset>>
{
    private TeamDocument? team;

    public override async Task Validate(RequestValidationContext<GetTeamAssetsRequest> context)
    {
        team = await teamRepository.GetById(context.Request.TeamId);
        team.ThrowApiExceptionIfNull(HttpStatusCode.NotFound);
    }

    public override async Task<List<TeamAsset>> Handle(GetTeamAssetsRequest request)
    {
        System.Collections.Generic.List<AssetDocument> assets = await assetRepository.GetByTeamId(team!.Id);
        
        List<TeamAsset> result = ListMapper.Map(assets, x => TeamAssetMapper.Map(x, team));

        return result;
    }
}