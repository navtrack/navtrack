using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Assets.Mappers;
using Navtrack.Api.Services.Common.Context;
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

[Service(typeof(IRequestHandler<CreateTeamAssetRequest>))]
public class CreateTeamAssetRequestHandler(
    ITeamRepository teamRepository,
    IAssetRepository assetRepository,
    INavtrackContextAccessor navtrackContextAccessor) : BaseRequestHandler<CreateTeamAssetRequest>
{
    private TeamDocument? team;
    private AssetDocument? asset;

    public override async Task Validate(RequestValidationContext<CreateTeamAssetRequest> context)
    {
        team = await teamRepository.GetById(context.Request.TeamId);
        team.Return404IfNull();

        asset = await assetRepository.GetById(context.Request.Model.AssetId);
        asset.Return404IfNull();

        if (team.OrganizationId != asset.OrganizationId)
        {
            throw new ApiException(ApiErrorCodes.Team_000002_TeamAndAssetNotInSameOrganization);
        }

        context.ValidationException.AddErrorIfTrue(
            asset.Teams?.Any(x => x.TeamId == team.Id),
            nameof(context.Request.Model.AssetId), ApiErrorCodes.Team_000003_AssetAlreadyInTeam);
    }

    public override async Task Handle(CreateTeamAssetRequest request)
    {
        AssetTeamElement element =
            AssetTeamElementMapper.Map(team!.Id, navtrackContextAccessor.NavtrackContext.User.Id);

        await assetRepository.AddAssetToTeam(asset!.Id, element);
        await teamRepository.UpdateAssetsCount(team!.Id);
    }

    public override IEvent GetEvent(CreateTeamAssetRequest request) =>
        new TeamAssetCreatedEvent
        {
            AssetId = asset!.Id.ToString(),
            TeamId = team!.Id.ToString()
        };
}