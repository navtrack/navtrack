using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Teams;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Teams;

[Service(typeof(IRequestHandler<DeleteTeamAssetRequest>))]
public class DeleteTeamAssetRequestHandler(ITeamRepository teamRepository, IAssetRepository assetRepository)
    : BaseRequestHandler<DeleteTeamAssetRequest>
{
    public override async Task Handle(DeleteTeamAssetRequest request)
    {
        TeamEntity? team = await teamRepository.GetById(request.TeamId);
        team.Return404IfNull();

        AssetEntity? asset = await assetRepository.GetById(request.AssetId);
        asset.Return404IfNull();

        if (asset.Teams.All(x => x.Id != team.Id))
        {
            throw new ApiException(ApiErrorCodes.Team_AssetNotInTeam);
        }
        
        await assetRepository.RemoveAssetFromTeam(team.Id, asset.Id);
        await teamRepository.UpdateAssetsCount(team.Id);
    }
}
