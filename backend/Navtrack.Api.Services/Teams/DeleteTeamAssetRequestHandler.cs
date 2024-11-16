using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.Teams.Events;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Teams;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.DataAccess.Services.Teams;
using Navtrack.Shared.Library.DI;
using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Teams;

[Service(typeof(IRequestHandler<DeleteTeamAssetRequest>))]
public class DeleteTeamAssetRequestHandler(ITeamRepository teamRepository, IAssetRepository assetRepository)
    : BaseRequestHandler<DeleteTeamAssetRequest>
{
    private TeamDocument? team;
    private AssetDocument? asset;
    
    public override async Task Validate(RequestValidationContext<DeleteTeamAssetRequest> context)
    {
        team = await teamRepository.GetById(context.Request.TeamId);
        team.Return404IfNull();

        asset = await assetRepository.GetById(context.Request.AssetId);
        asset.Return404IfNull();

        if (asset.Teams?.All(x => x.TeamId != team.Id) ?? false)
        {
            throw new ApiException(ApiErrorCodes.Team_000005_AssetNotInTeam);
        }
    }

    public override async Task Handle(DeleteTeamAssetRequest request)
    {
        await assetRepository.RemoveAssetFromTeam(team!.Id, asset!.Id);
        await teamRepository.UpdateAssetsCount(team!.Id);
    }
    
    public override IEvent GetEvent(DeleteTeamAssetRequest request) =>
        new TeamAssetDeletedEvent
        {
            AssetId = asset!.Id.ToString(),
            TeamId = team!.Id.ToString()
        };
}